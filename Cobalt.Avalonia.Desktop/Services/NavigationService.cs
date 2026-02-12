using Avalonia.Controls;
using Cobalt.Avalonia.Desktop.Controls.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cobalt.Avalonia.Desktop.Services;

public class NavigationService : ObservableObject, INavigationService
{
    private readonly SemaphoreSlim _navigationLock = new(1, 1);

    public NavigationService(IReadOnlyList<NavigationItemControl> items, IReadOnlyList<NavigationItemControl>? footerItems = null)
    {
        Items = items;
        FooterItems = footerItems;
    }

    public Control? CurrentPage
    {
        get;
        set => SetProperty(ref field, value);
    }

    public NavigationItemControl? SelectedItem
    {
        get;
        set
        {
            var previousItem = field;
            if (SetProperty(ref field, value))
                _ = TryNavigateToItemAsync(value, previousItem);
        }
    }

    public IReadOnlyList<NavigationItemControl> Items { get; }
    public IReadOnlyList<NavigationItemControl>? FooterItems { get; }

    public async Task NavigateToAsync(Control page)
    {
        if (!await _navigationLock.WaitAsync(0))
            return;

        try
        {
            var context = new NavigationContext { TargetPage = page };

            bool allowed = await InvokeDisappearingAsync(CurrentPage, context);
            if (!allowed)
                return; // Navigation cancelled

            CurrentPage = page;

            SelectedItem = FindItemForPage(page);

            await InvokeAppearingAsync(CurrentPage);
        }
        finally
        {
            _navigationLock.Release();
        }
    }

    private NavigationItemControl? FindItemForPage(Control page)
    {
        var pageType = page.GetType();

        foreach (var item in Items)
        {
            if (item.PageType == pageType)
                return item;
        }

        if (FooterItems != null)
        {
            foreach (var item in FooterItems)
            {
                if (item.PageType == pageType)
                    return item;
            }
        }

        return null;
    }

    private async Task TryNavigateToItemAsync(NavigationItemControl? targetItem, NavigationItemControl? previousItem)
    {
        // Prevent concurrent navigations
        if (!await _navigationLock.WaitAsync(0))
            return;

        try
        {
            var targetPage = targetItem?.Factory?.Invoke();

            var context = new NavigationContext
            {
                TargetPage = targetPage,
                TargetItem = targetItem
            };

            // Check if current page allows navigation (async)
            bool allowed = await InvokeDisappearingAsync(CurrentPage, context);

            if (!allowed)
            {
                // Navigation cancelled - restore previous selection
                SelectedItem = previousItem;
                return;
            }

            // Navigation allowed - _selectedItem is already set to targetItem from setter
            CurrentPage = targetPage;

            // Call OnAppearing on new page (async)
            await InvokeAppearingAsync(CurrentPage);
        }
        finally
        {
            _navigationLock.Release();
        }
    }

    private async Task<bool> InvokeDisappearingAsync(object? page, NavigationContext context)
    {
        if (page == null)
            return true; // Allow navigation from null page

        bool allowNavigation = true;

        // Check View first
        if (page is Control control)
        {
            if (control is INavigationLifecycleAsync asyncView)
            {
                try
                {
                    allowNavigation = await asyncView.OnDisappearingAsync(context);
                }
                catch
                {
                    // Log exception but allow navigation (fail-safe)
                    // Exception = unexpected state, proceed with caution
                }

                if (!allowNavigation)
                    return false; // View cancelled, don't check ViewModel
            }

            // Check ViewModel (only if View allowed or had no opinion)
            if (allowNavigation && control.DataContext != null)
            {
                if (control.DataContext is INavigationLifecycleAsync asyncViewModel)
                {
                    try
                    {
                        allowNavigation = await asyncViewModel.OnDisappearingAsync(context);
                    }
                    catch
                    {
                        // Log exception but allow navigation
                    }
                }
            }
        }

        return allowNavigation;
    }

    private async Task InvokeAppearingAsync(object? page)
    {
        if (page == null)
            return;

        // Try View
        if (page is Control control)
        {
            if (control is INavigationLifecycleAsync asyncView)
            {
                try
                {
                    await asyncView.OnAppearingAsync();
                }
                catch
                {
                    // Log exception but continue
                }
            }

            // Try ViewModel
            if (control.DataContext != null)
            {
                if (control.DataContext is INavigationLifecycleAsync asyncViewModel)
                {
                    try
                    {
                        await asyncViewModel.OnAppearingAsync();
                    }
                    catch
                    {
                        // Log exception but continue
                    }
                }
            }
        }
    }
}
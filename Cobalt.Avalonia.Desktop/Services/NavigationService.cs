using Avalonia.Controls;
using Cobalt.Avalonia.Desktop.Controls.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cobalt.Avalonia.Desktop.Services;

public class NavigationService(
    IReadOnlyList<NavigationItemControl> items,
    IReadOnlyList<NavigationItemControl>? footerItems = null)
    : ObservableObject, INavigationService
{
    private readonly SemaphoreSlim _navigationLock = new(1, 1);

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

    public IReadOnlyList<NavigationItemControl> Items { get; } = items;
    public IReadOnlyList<NavigationItemControl>? FooterItems { get; } = footerItems;

    public async Task NavigateToAsync(Control page)
    {
        if (!await _navigationLock.WaitAsync(0))
            return;

        try
        {
            var context = new NavigationContext { TargetPage = page };

            if (CurrentPage is not null)
            {
                var allowed = await InvokeDisappearingAsync(CurrentPage, context);
                if (!allowed)
                    return; // Navigation cancelled
            }

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

            if (CurrentPage is not null)
            {
                // Check if current page allows navigation (async)
                bool allowed = await InvokeDisappearingAsync(CurrentPage, context);

                if (!allowed)
                {
                    // Navigation cancelled - restore previous selection
                    SelectedItem = previousItem;
                    return;
                }
            }

            // Navigation allowed - _selectedItem is already set to targetItem from setter
            CurrentPage = targetPage;

            // Call OnAppearing on new page (async)
            if (CurrentPage is not null)
                await InvokeAppearingAsync(CurrentPage);
        }
        finally
        {
            _navigationLock.Release();
        }
    }

    private async Task<bool> InvokeDisappearingAsync(Control page, NavigationContext context)
    {
        var allowNavigation = true;

        switch (page.DataContext)
        {
            case null:
                return true;
            case INavigationLifecycleAsync asyncViewModel:
                try
                {
                    allowNavigation = await asyncViewModel.OnDisappearingAsync(context);
                }
                catch
                {
                    // Log exception but allow navigation
                }

                break;
        }

        return allowNavigation;
    }

    private async Task InvokeAppearingAsync(Control page)
    {
        if (page.DataContext is INavigationLifecycleAsync asyncViewModel)
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
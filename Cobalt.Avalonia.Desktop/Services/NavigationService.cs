using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Cobalt.Avalonia.Desktop.Controls.Navigation;

namespace Cobalt.Avalonia.Desktop.Services;

public class NavigationService : INavigationService, INotifyPropertyChanged
{
    private object? _currentPage;
    private NavigationItemControl? _selectedItem;
    private bool _isNavigating;
    private readonly SemaphoreSlim _navigationLock = new(1, 1);

    public event PropertyChangedEventHandler? PropertyChanged;

    public object? CurrentPage
    {
        get => _currentPage;
        private set
        {
            if (_currentPage != value)
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }
    }

    public NavigationItemControl? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value && !_isNavigating)
            {
                // Launch async navigation (fire-and-forget with proper error handling)
                _ = TryNavigateToItemAsync(value);
            }
        }
    }

    public IReadOnlyList<NavigationItemControl> Items { get; }
    public IReadOnlyList<NavigationItemControl>? FooterItems { get; }

    public NavigationService(IReadOnlyList<NavigationItemControl> items, IReadOnlyList<NavigationItemControl>? footerItems = null)
    {
        Items = items;
        FooterItems = footerItems;
    }

    public async Task NavigateTo(Control page)
    {
        if (!await _navigationLock.WaitAsync(0))
            return;

        try
        {
            _isNavigating = true;

            var previousPage = _currentPage;
            var context = new NavigationContext { TargetPage = page };

            bool allowed = await InvokeDisappearingAsync(_currentPage, context);
            if (!allowed)
                return; // Navigation cancelled

            CurrentPage = page;

            _selectedItem = FindItemForPage(page);
            OnPropertyChanged(nameof(SelectedItem));

            await InvokeAppearingAsync(_currentPage);
        }
        finally
        {
            _isNavigating = false;
            _navigationLock.Release();
        }
    }

    public void NavigateToItem(NavigationItemControl item) => SelectedItem = item;

    public async Task NavigateToAsync(Control page)
    {
        await NavigateTo(page);
    }

    public Task NavigateToItemAsync(int index)
    {
        NavigateToItem(Items[index]);
        return Task.CompletedTask;
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

    private async Task TryNavigateToItemAsync(NavigationItemControl? targetItem)
    {
        // Prevent concurrent navigations
        if (!await _navigationLock.WaitAsync(0))
            return;

        try
        {
            _isNavigating = true;

            var previousItem = _selectedItem;
            var previousPage = _currentPage;
            var targetPage = targetItem?.Factory?.Invoke();

            var context = new NavigationContext
            {
                TargetPage = targetPage,
                TargetItem = targetItem
            };

            // Check if current page allows navigation (async)
            bool allowed = await InvokeDisappearingAsync(_currentPage, context);

            if (!allowed)
            {
                // Navigation cancelled - restore previous selection
                _selectedItem = previousItem;
                OnPropertyChanged(nameof(SelectedItem));
                return;
            }

            // Navigation allowed - proceed
            _selectedItem = targetItem;
            OnPropertyChanged(nameof(SelectedItem));

            CurrentPage = targetPage;

            // Call OnAppearing on new page (async)
            await InvokeAppearingAsync(_currentPage);
        }
        finally
        {
            _isNavigating = false;
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
            else if (control is INavigationLifecycle syncView)
            {
                // Fallback to sync version (no cancellation support)
                try
                {
                    syncView.OnDisappearing();
                }
                catch
                {
                    // Swallow exceptions as before
                }
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
                else if (control.DataContext is INavigationLifecycle syncViewModel)
                {
                    // Fallback to sync version (no cancellation support)
                    try
                    {
                        syncViewModel.OnDisappearing();
                    }
                    catch
                    {
                        // Swallow exceptions as before
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
            else if (control is INavigationLifecycle syncView)
            {
                try
                {
                    syncView.OnAppearing();
                }
                catch
                {
                    // Swallow exceptions
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
                else if (control.DataContext is INavigationLifecycle syncViewModel)
                {
                    try
                    {
                        syncViewModel.OnAppearing();
                    }
                    catch
                    {
                        // Swallow exceptions
                    }
                }
            }
        }
    }

    /// <summary>
    /// Invokes a lifecycle method on both the page (if it's a Control) and its DataContext (ViewModel).
    /// Handles null checks and exception swallowing to prevent lifecycle errors from breaking navigation.
    /// </summary>
    private void InvokeLifecycleMethod(object? page, Action<INavigationLifecycle> action)
    {
        if (page == null)
            return;

        // Try to invoke on the page itself (View)
        if (page is INavigationLifecycle lifecyclePage)
        {
            try
            {
                action(lifecyclePage);
            }
            catch
            {
                // Swallow exceptions to prevent lifecycle errors from breaking navigation
                // In production, consider logging here
            }
        }

        // Try to invoke on the DataContext (ViewModel)
        if (page is Control control && control.DataContext is INavigationLifecycle lifecycleViewModel)
        {
            try
            {
                action(lifecycleViewModel);
            }
            catch
            {
                // Swallow exceptions to prevent lifecycle errors from breaking navigation
                // In production, consider logging here
            }
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

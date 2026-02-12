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

    public event PropertyChangedEventHandler? PropertyChanged;

    public object? CurrentPage
    {
        get => _currentPage;
        private set
        {
            if (_currentPage != value)
            {
                // Call OnDisappearing on the OLD page (before the change)
                InvokeLifecycleMethod(_currentPage, lifecycle => lifecycle.OnDisappearing());

                _currentPage = value;
                OnPropertyChanged();

                // Call OnAppearing on the NEW page (after the change)
                InvokeLifecycleMethod(_currentPage, lifecycle => lifecycle.OnAppearing());
            }
        }
    }

    public NavigationItemControl? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged();

                if (!_isNavigating)
                    CurrentPage = value?.Factory?.Invoke();
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

    public void NavigateTo(Control page)
    {
        _isNavigating = true;
        try
        {
            CurrentPage = page;
            SelectedItem = FindItemForPage(page);
        }
        finally
        {
            _isNavigating = false;
        }
    }

    public void NavigateToItem(NavigationItemControl item) => SelectedItem = item;

    public Task NavigateToAsync(Control page)
    {
        NavigateTo(page);
        return Task.CompletedTask;
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

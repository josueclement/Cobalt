using Cobalt.Avalonia.Desktop.Controls.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Cobalt.Avalonia.Desktop.Services;

/// <summary>
/// Implementation of the navigation service for managing application navigation state and lifecycle.
/// </summary>
/// <param name="serviceProvider">The DI service provider used to resolve ViewModel instances.</param>
public class NavigationService(IServiceProvider serviceProvider)
    : ObservableObject, INavigationService
{
    /// <summary>
    /// Semaphore used to synchronize navigation operations and prevent concurrent navigations.
    /// </summary>
    private readonly SemaphoreSlim _navigationLock = new(1, 1);

    /// <summary>
    /// Gets the currently displayed page ViewModel.
    /// </summary>
    public object? CurrentPage
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Gets or sets the currently selected navigation item.
    /// Changing this property triggers navigation to the selected item.
    /// </summary>
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

    /// <summary>
    /// Gets the main navigation items.
    /// </summary>
    public IReadOnlyList<NavigationItemControl> Items { get; private set; } = [];

    /// <summary>
    /// Gets the footer navigation items.
    /// </summary>
    public IReadOnlyList<NavigationItemControl>? FooterItems { get; private set; }

    /// <summary>
    /// Initializes the navigation service with the provided navigation items.
    /// </summary>
    /// <param name="items">The main navigation items.</param>
    /// <param name="footerItems">The footer navigation items.</param>
    public void Initialize(IReadOnlyList<NavigationItemControl> items, IReadOnlyList<NavigationItemControl>? footerItems = null)
    {
        Items = items;
        FooterItems = footerItems;
    }

    /// <summary>
    /// Navigates to a page by resolving the specified ViewModel type from DI.
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel type to navigate to.</typeparam>
    public Task NavigateToAsync<TViewModel>() where TViewModel : class
    {
        return NavigateToAsync(typeof(TViewModel));
    }

    /// <summary>
    /// Navigates to a page by resolving the specified ViewModel type from DI.
    /// </summary>
    /// <param name="viewModelType">The ViewModel type to navigate to.</param>
    public async Task NavigateToAsync(Type viewModelType)
    {
        if (!await _navigationLock.WaitAsync(0))
            return;

        try
        {
            if (CurrentPage is not null)
            {
                var allowed = await InvokeDisappearingAsync(CurrentPage);
                if (!allowed)
                    return;
            }

            CurrentPage = serviceProvider.GetRequiredService(viewModelType);

            SelectedItem = FindItemForViewModel(viewModelType);

            await InvokeAppearingAsync(CurrentPage);
        }
        finally
        {
            _navigationLock.Release();
        }
    }

    /// <summary>
    /// Finds the navigation item that corresponds to the given ViewModel type.
    /// </summary>
    /// <param name="viewModelType">The ViewModel type to find an item for.</param>
    /// <returns>The matching <see cref="NavigationItemControl"/> if found; otherwise, <c>null</c>.</returns>
    private NavigationItemControl? FindItemForViewModel(Type viewModelType)
    {
        foreach (var item in Items)
        {
            if (item.PageViewModelType == viewModelType)
                return item;
        }

        if (FooterItems != null)
        {
            foreach (var item in FooterItems)
            {
                if (item.PageViewModelType == viewModelType)
                    return item;
            }
        }

        return null;
    }

    /// <summary>
    /// Attempts to navigate to the specified navigation item.
    /// </summary>
    /// <param name="targetItem">The target navigation item.</param>
    /// <param name="previousItem">The previously selected navigation item, to be restored if navigation is cancelled.</param>
    private async Task TryNavigateToItemAsync(NavigationItemControl? targetItem, NavigationItemControl? previousItem)
    {
        if (!await _navigationLock.WaitAsync(0))
            return;

        try
        {
            if (CurrentPage is not null)
            {
                var allowed = await InvokeDisappearingAsync(CurrentPage);

                if (!allowed)
                {
                    // Navigation canceled - restore previous selection
                    SelectedItem = previousItem;
                    return;
                }
            }

            if (targetItem?.PageViewModelType is { } vmType)
                CurrentPage = serviceProvider.GetRequiredService(vmType);
            else
                CurrentPage = null;

            if (CurrentPage is not null)
                await InvokeAppearingAsync(CurrentPage);
        }
        finally
        {
            _navigationLock.Release();
        }
    }

    /// <summary>
    /// Invokes the <see cref="INavigationViewModel.OnDisappearingAsync"/> method on the ViewModel if it implements the interface.
    /// </summary>
    /// <param name="viewModel">The current page ViewModel.</param>
    /// <returns><c>true</c> if navigation is allowed; otherwise, <c>false</c>.</returns>
    private static async Task<bool> InvokeDisappearingAsync(object viewModel)
    {
        try
        {
            if (viewModel is INavigationViewModel nav)
                return await nav.OnDisappearingAsync();
            return true;
        }
        catch
        {
            return true;
        }
    }

    /// <summary>
    /// Invokes the <see cref="INavigationViewModel.OnAppearingAsync"/> method on the ViewModel if it implements the interface.
    /// </summary>
    /// <param name="viewModel">The current page ViewModel.</param>
    private static async Task InvokeAppearingAsync(object viewModel)
    {
        try
        {
            if (viewModel is INavigationViewModel nav)
                await nav.OnAppearingAsync();
        }
        catch
        {
            // ignored
        }
    }
}

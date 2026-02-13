namespace Cobalt.Avalonia.Desktop;

/// <summary>
/// Defines a service for managing navigation within the application.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Gets the currently displayed page ViewModel.
    /// </summary>
    object? CurrentPage { get; }

    /// <summary>
    /// Navigates to a page by resolving the specified ViewModel type from DI.
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel type to navigate to.</typeparam>
    Task NavigateToAsync<TViewModel>() where TViewModel : class;

    /// <summary>
    /// Navigates to a page by resolving the specified ViewModel type from DI.
    /// </summary>
    /// <param name="viewModelType">The ViewModel type to navigate to.</param>
    Task NavigateToAsync(Type viewModelType);
}

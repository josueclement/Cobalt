using Avalonia.Controls;

namespace Cobalt.Avalonia.Desktop;

/// <summary>
/// Defines a service for managing navigation within the application.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Gets the currently displayed page Control.
    /// </summary>
    Control? CurrentPage { get; }

    /// <summary>
    /// Navigates to the specified page Control.
    /// </summary>
    /// <param name="page">The page Control to navigate to (with DataContext already set).</param>
    Task NavigateToAsync(Control page);
}

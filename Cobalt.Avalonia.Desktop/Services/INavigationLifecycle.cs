namespace Cobalt.Avalonia.Desktop.Services;

/// <summary>
/// Defines lifecycle methods for navigation pages.
/// Can be implemented by either page views (Controls) or their ViewModels.
/// </summary>
public interface INavigationLifecycle
{
    /// <summary>
    /// Called when the page is about to disappear from view.
    /// This is invoked BEFORE the CurrentPage property changes.
    /// Use this to clean up resources, save state, or stop background operations.
    /// </summary>
    void OnDisappearing();

    /// <summary>
    /// Called when the page has appeared and is now visible.
    /// This is invoked AFTER the CurrentPage property changes.
    /// Use this to load data, start animations, or initialize the page.
    /// </summary>
    void OnAppearing();
}

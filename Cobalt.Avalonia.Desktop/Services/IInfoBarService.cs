using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

/// <summary>
/// Defines a service for displaying and managing info bars in the application.
/// InfoBarControl instances must be registered via <see cref="RegisterHost"/> before showing info bars.
/// </summary>
public interface IInfoBarService
{
    /// <summary>
    /// Registers the host InfoBarControl that will be used to display all info bars.
    /// This must be called once during application initialization before showing any info bars.
    /// </summary>
    /// <param name="infoBar">The InfoBarControl to register as the host.</param>
    void RegisterHost(InfoBarControl infoBar);

    /// <summary>
    /// Shows an info bar with optional configuration.
    /// The info bar is reset to default state before the configuration action is applied.
    /// </summary>
    /// <param name="configure">An optional action that configures the InfoBarControl properties before showing it.</param>
    /// <returns>A task that represents the asynchronous show operation.</returns>
    Task ShowAsync(Action<InfoBarControl>? configure = null);

    /// <summary>
    /// Hides the currently displayed info bar, if any.
    /// </summary>
    /// <returns>A task that represents the asynchronous hide operation.</returns>
    Task HideAsync();
}

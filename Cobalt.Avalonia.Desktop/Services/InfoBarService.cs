using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

/// <summary>
/// Implementation of the info bar service for displaying notification messages in the application.
/// Manages a single InfoBarControl host and resets it between uses.
/// </summary>
public class InfoBarService : IInfoBarService
{
    /// <summary>
    /// The registered InfoBarControl used to display all info bars.
    /// </summary>
    private InfoBarControl? _host;

    /// <summary>
    /// Registers the host InfoBarControl that will be used to display all info bars.
    /// This must be called once during application initialization before showing any info bars.
    /// </summary>
    /// <param name="infoBar">The InfoBarControl to register as the host.</param>
    public void RegisterHost(InfoBarControl infoBar)
    {
        _host = infoBar;
    }

    /// <summary>
    /// Shows an info bar with optional configuration.
    /// The info bar is reset to default state before the configuration action is applied.
    /// </summary>
    /// <param name="configure">An optional action that configures the InfoBarControl properties before showing it.</param>
    /// <returns>A task that represents the asynchronous show operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no InfoBarControl host has been registered.</exception>
    public async Task ShowAsync(Action<InfoBarControl>? configure = null)
    {
        if (_host is null)
            throw new InvalidOperationException("InfoBarControl host has not been registered. Call RegisterHost first.");

        ResetInfoBar(_host);
        configure?.Invoke(_host);
        await _host.ShowAsync();
    }

    /// <summary>
    /// Hides the currently displayed info bar, if any, by calling its CloseAsync method.
    /// </summary>
    /// <returns>A task that represents the asynchronous hide operation.</returns>
    public async Task HideAsync()
    {
        if (_host is not null)
            await _host.CloseAsync();
    }

    /// <summary>
    /// Resets all properties of the specified InfoBarControl to their default values.
    /// This ensures each info bar starts with a clean state.
    /// </summary>
    /// <param name="infoBar">The InfoBarControl to reset.</param>
    private static void ResetInfoBar(InfoBarControl infoBar)
    {
        infoBar.Title = null;
        infoBar.Message = null;
        infoBar.Severity = InfoBarSeverity.Info;
    }
}

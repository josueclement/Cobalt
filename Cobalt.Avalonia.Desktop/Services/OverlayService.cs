using Avalonia.Controls;
using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public class OverlayService : IOverlayService
{
    private OverlayPresenter? _host;

    public void RegisterHost(OverlayPresenter presenter)
    {
        _host = presenter;
    }

    public Task ShowAsync(Control control)
    {
        if (_host is null)
            throw new InvalidOperationException("No OverlayPresenter registered. Call RegisterHost first.");

        if (control is null)
            throw new ArgumentNullException(nameof(control));

        _host.Content = control;
        _host.IsOpen = true;
        return Task.CompletedTask;
    }

    public Task HideAsync()
    {
        if (_host is not null)
        {
            _host.IsOpen = false;
            _host.Content = null;  // Clear content to prevent memory leaks
        }
        return Task.CompletedTask;
    }
}

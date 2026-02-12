using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public interface IOverlayService
{
    Task ShowAsync(Action<OverlayControl>? configure = null);
    void Update(Action<OverlayControl> configure);
    Task HideAsync();
}

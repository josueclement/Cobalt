using Avalonia.Controls;
using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public interface IOverlayService
{
    void RegisterHost(OverlayPresenter presenter);
    Task ShowAsync(Control control);
    Task HideAsync();
}

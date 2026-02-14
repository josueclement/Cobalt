using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public interface IInfoBarService
{
    void RegisterHost(InfoBarControl infoBar);
    Task ShowAsync(Action<InfoBarControl>? configure = null);
    Task HideAsync();
}

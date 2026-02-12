using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public interface IInfoBarService
{
    // TODO: What is this action here ?
    Task ShowAsync(Action<InfoBarControl>? configure = null);
    Task HideAsync();
}

using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public class InfoBarService : IInfoBarService
{
    private InfoBarControl? _host;

    public void RegisterHost(InfoBarControl infoBar)
    {
        _host = infoBar;
    }

    public async Task ShowAsync(Action<InfoBarControl>? configure = null)
    {
        if (_host is null)
            throw new InvalidOperationException("InfoBarControl host has not been registered. Call RegisterHost first.");

        ResetInfoBar(_host);
        configure?.Invoke(_host);
        await _host.ShowAsync();
    }

    public void Hide()
    {
        _host?.Close();
    }

    private static void ResetInfoBar(InfoBarControl infoBar)
    {
        infoBar.Title = null;
        infoBar.Message = null;
        infoBar.Severity = InfoBarSeverity.Info;
    }
}

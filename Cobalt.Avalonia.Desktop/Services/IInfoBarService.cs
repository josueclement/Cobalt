using System;
using System.Threading.Tasks;
using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public interface IInfoBarService
{
    Task ShowAsync(Action<InfoBarControl>? configure = null);
    void Hide();
}

using System;
using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public interface IOverlayService
{
    void Show(Action<OverlayControl>? configure = null);
    void Update(Action<OverlayControl> configure);
    void Hide();
}

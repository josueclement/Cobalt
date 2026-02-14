using Avalonia;
using Avalonia.Controls;

namespace Cobalt.Avalonia.Desktop.Controls;

public class OverlayPresenter : ContentControl
{
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<OverlayPresenter, bool>(nameof(IsOpen), false);

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }
}

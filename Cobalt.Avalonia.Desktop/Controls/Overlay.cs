using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls;

public class Overlay : ContentControl
{
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<Overlay, bool>(nameof(IsOpen), false);

    public static readonly StyledProperty<IBrush?> OverlayBrushProperty =
        AvaloniaProperty.Register<Overlay, IBrush?>(
            nameof(OverlayBrush),
            new SolidColorBrush(Color.FromArgb(77, 0, 0, 0)));

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public IBrush? OverlayBrush
    {
        get => GetValue(OverlayBrushProperty);
        set => SetValue(OverlayBrushProperty, value);
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed partial class LineShape : Shape
{
    [ObservableProperty] private double _x2;
    [ObservableProperty] private double _y2;

    public override void Render(DrawingContext context)
    {
        var pen = BuildPen();
        if (pen is null) return;
        using var _ = context.PushTransform(RenderTransform);
        context.DrawLine(pen, new global::Avalonia.Point(X, Y),
                              new global::Avalonia.Point(X2, Y2));
    }
}

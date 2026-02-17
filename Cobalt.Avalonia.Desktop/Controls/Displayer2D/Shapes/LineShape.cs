using CommunityToolkit.Mvvm.ComponentModel;
using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed partial class LineShape : Shape
{
    [ObservableProperty] private double _x2;
    [ObservableProperty] private double _y2;

    public double CanvasX2 { get; private set; }
    public double CanvasY2 { get; private set; }

    protected override void RecalculateExtraCoordinates(double zoom, double panX, double panY)
    {
        CanvasX2 = IsFixed ? X2 : X2 * zoom + panX;
        CanvasY2 = IsFixed ? Y2 : Y2 * zoom + panY;
    }

    public override void Render(DrawingContext context)
    {
        var pen = BuildPen();
        if (pen is null) return;
        context.DrawLine(pen, new global::Avalonia.Point(CanvasX, CanvasY),
                              new global::Avalonia.Point(CanvasX2, CanvasY2));
    }
}

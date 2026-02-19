using Avalonia;
using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed class LineShape : Shape
{
    public double X2 { get; set { SetProperty(ref field, value); MarkCoordinatesDirty(); } }
    public double Y2 { get; set { SetProperty(ref field, value); MarkCoordinatesDirty(); } }

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
        context.DrawLine(pen, new Point(CanvasX, CanvasY),
                              new Point(CanvasX2, CanvasY2));
    }

    // Point-to-segment distance, 5px tolerance for easier hovering
    public override bool HitTest(Point p)
    {
        const double tolerance = 5.0;
        var ax = CanvasX2 - CanvasX;
        var ay = CanvasY2 - CanvasY;
        var lenSq = ax * ax + ay * ay;
        if (lenSq == 0) return Math.Abs(p.X - CanvasX) <= tolerance && Math.Abs(p.Y - CanvasY) <= tolerance;
        var t = Math.Clamp(((p.X - CanvasX) * ax + (p.Y - CanvasY) * ay) / lenSq, 0, 1);
        var dx = p.X - (CanvasX + t * ax);
        var dy = p.Y - (CanvasY + t * ay);
        return dx * dx + dy * dy <= tolerance * tolerance;
    }
}

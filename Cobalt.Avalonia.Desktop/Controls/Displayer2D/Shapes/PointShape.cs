using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed partial class PointShape : Shape
{
    public PointShape()
    {
        Width = 6.0;
        Height = 6.0;
    }

    public override void Render(DrawingContext context)
    {
        context.DrawEllipse(EffectiveFill, BuildPen(),
            new global::Avalonia.Point(CanvasX, CanvasY),
            Width / 2, Height / 2);
    }

    // CanvasX/CanvasY is the center, not top-left
    public override bool HitTest(Point canvasPoint) =>
        Math.Abs(canvasPoint.X - CanvasX) <= CanvasWidth  / 2 &&
        Math.Abs(canvasPoint.Y - CanvasY) <= CanvasHeight / 2;
}

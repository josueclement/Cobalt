using Avalonia;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public class CircleShape : EllipseShape
{
    public CircleShape()
    {
        Width  = 6.0;
        Height = 6.0;
    }

    // Circles are rotation-invariant: use exact radial distance (no trig needed).
    public override bool HitTest(Point canvasPoint)
    {
        var dx = canvasPoint.X - (CanvasX + CanvasWidth  / 2);
        var dy = canvasPoint.Y - (CanvasY + CanvasHeight / 2);
        var r  = CanvasWidth / 2;
        return dx * dx + dy * dy <= r * r;
    }
}

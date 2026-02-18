using Avalonia;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public partial class CircleShape : EllipseShape
{
    public CircleShape()
    {
        Width  = 6.0;
        Height = 6.0;
    }

    // CanvasX/CanvasY is the top-left; center is offset by half the canvas size
    public override bool HitTest(Point canvasPoint) =>
        Math.Abs(canvasPoint.X - (CanvasX + CanvasWidth  / 2)) <= CanvasWidth  / 2 &&
        Math.Abs(canvasPoint.Y - (CanvasY + CanvasHeight / 2)) <= CanvasHeight / 2;
}

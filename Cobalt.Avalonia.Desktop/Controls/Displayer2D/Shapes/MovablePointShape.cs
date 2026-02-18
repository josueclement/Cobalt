using System;
using Avalonia;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed class MovablePointShape : PointShape, IMovableDrawingObject
{
    public MovablePointShape() { Width = 14.0; Height = 14.0; }

    public event EventHandler<MovedEventArgs>? Moved;

    public void Move(double deltaX, double deltaY)
    {
        X += deltaX;
        Y += deltaY;
        Moved?.Invoke(this, new MovedEventArgs { DeltaX = deltaX, DeltaY = deltaY, NewX = X, NewY = Y });
    }

    public override bool HitTest(Point canvasPoint) =>
        Math.Abs(canvasPoint.X - (CanvasX + CanvasWidth  / 2)) <= CanvasWidth  / 2 &&
        Math.Abs(canvasPoint.Y - (CanvasY + CanvasHeight / 2)) <= CanvasHeight / 2;
}

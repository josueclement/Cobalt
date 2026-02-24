using Avalonia;
using Avalonia.Input;
using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed class MovedEventArgs : EventArgs
{
    public double DeltaX { get; init; }
    public double DeltaY { get; init; }
    public double NewX { get; init; }
    public double NewY { get; init; }
}

public abstract class Shape : DrawingObject
{
    public bool IsMovable { get; set; }

    public event EventHandler<MovedEventArgs>? Moved;

    public void Move(double deltaX, double deltaY)
    {
        X += deltaX;
        Y += deltaY;
        Moved?.Invoke(this, new MovedEventArgs { DeltaX = deltaX, DeltaY = deltaY, NewX = X, NewY = Y });
    }

    public IBrush? Fill { get; set => SetProperty(ref field, value); }
    public IBrush? Stroke { get; set => SetProperty(ref field, value); }
    public double StrokeThickness { get; set => SetProperty(ref field, value); } = 1.0;
    public IBrush? FillHover { get; set => SetProperty(ref field, value); }
    public IBrush? StrokeHover { get; set => SetProperty(ref field, value); }
    public IReadOnlyList<double>? StrokeDashArray { get; set; }
    public Cursor? Cursor { get; set; }

    internal bool IsHovered { get; set; }

    protected IBrush? EffectiveFill => IsHovered && FillHover is not null ? FillHover : Fill;
    protected IBrush? EffectiveStroke => IsHovered && StrokeHover is not null ? StrokeHover : Stroke;

    protected IPen? BuildPen()
    {
        if (EffectiveStroke is null) return null;
        var dash = StrokeDashArray is not null ? new DashStyle(StrokeDashArray, 0) : null;
        return new Pen(EffectiveStroke, StrokeThickness, dashStyle: dash);
    }

    /// <summary>Returns true if the given canvas-space point is over this shape.</summary>
    /// <remarks>
    /// Inverse-rotates the point into local (unrotated) space, then performs an AABB check.
    /// Subclasses with non-rectangular geometry (ellipse, line…) should override.
    /// </remarks>
    public virtual bool HitTest(Point canvasPoint)
    {
        var dx = canvasPoint.X - (CanvasX + CanvasWidth  / 2);
        var dy = canvasPoint.Y - (CanvasY + CanvasHeight / 2);

        if (Rotation != 0.0)
        {
            var rad = -Rotation * Math.PI / 180.0;
            var cos = Math.Cos(rad);
            var sin = Math.Sin(rad);
            (dx, dy) = (cos * dx - sin * dy, sin * dx + cos * dy);
        }

        return Math.Abs(dx) <= CanvasWidth  / 2 &&
               Math.Abs(dy) <= CanvasHeight / 2;
    }
}

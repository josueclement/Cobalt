using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public abstract class Shape : DrawingObject
{
    public IBrush? Fill           { get; set => SetProperty(ref field, value); }
    public IBrush? Stroke         { get; set => SetProperty(ref field, value); }
    public double  StrokeThickness { get; set => SetProperty(ref field, value); } = 1.0;

    public IBrush? FillHover   { get; set => SetProperty(ref field, value); }
    public IBrush? StrokeHover { get; set => SetProperty(ref field, value); }

    internal bool IsHovered { get; set; }

    protected IBrush? EffectiveFill   => IsHovered && FillHover   is not null ? FillHover   : Fill;
    protected IBrush? EffectiveStroke => IsHovered && StrokeHover is not null ? StrokeHover : Stroke;

    protected IPen? BuildPen() => EffectiveStroke is null ? null : new Pen(EffectiveStroke, StrokeThickness);

    /// <summary>Returns true if the given canvas-space point is over this shape.</summary>
    public virtual bool HitTest(Point canvasPoint) =>
        canvasPoint.X >= CanvasX && canvasPoint.X <= CanvasX + CanvasWidth &&
        canvasPoint.Y >= CanvasY && canvasPoint.Y <= CanvasY + CanvasHeight;
}

using CommunityToolkit.Mvvm.ComponentModel;
using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public abstract partial class Shape : DrawingObject
{
    [ObservableProperty] private IBrush? _fill;
    [ObservableProperty] private IBrush? _stroke;
    [ObservableProperty] private double _strokeThickness = 1.0;

    [ObservableProperty] private IBrush? _fillHover;
    [ObservableProperty] private IBrush? _strokeHover;

    internal bool IsHovered { get; set; }

    protected IBrush? EffectiveFill   => IsHovered && FillHover   is not null ? FillHover   : Fill;
    protected IBrush? EffectiveStroke => IsHovered && StrokeHover is not null ? StrokeHover : Stroke;

    protected IPen? BuildPen() => EffectiveStroke is null ? null : new Pen(EffectiveStroke, StrokeThickness);

    /// <summary>Returns true if the given canvas-space point is over this shape.</summary>
    public virtual bool HitTest(Point canvasPoint) =>
        canvasPoint.X >= CanvasX && canvasPoint.X <= CanvasX + CanvasWidth &&
        canvasPoint.Y >= CanvasY && canvasPoint.Y <= CanvasY + CanvasHeight;
}

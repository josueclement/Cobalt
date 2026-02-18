using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public partial class EllipseShape : Shape
{
    public double CenterX { get => X + Width / 2;  set => X = value - Width  / 2; }
    public double CenterY { get => Y + Height / 2; set => Y = value - Height / 2; }

    public override void Render(DrawingContext context)
    {
        using var _ = PushRotation(context);
        context.DrawEllipse(EffectiveFill, BuildPen(),
            new global::Avalonia.Point(CanvasX + CanvasWidth / 2, CanvasY + CanvasHeight / 2),
            CanvasWidth / 2, CanvasHeight / 2);
    }
}

using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed partial class EllipseShape : Shape
{
    public override void Render(DrawingContext context)
    {
        using var _ = context.PushTransform(RenderTransform);
        context.DrawEllipse(Fill, BuildPen(),
            new global::Avalonia.Point(X + Width / 2, Y + Height / 2),
            Width / 2, Height / 2);
    }
}

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
        using var _ = context.PushTransform(RenderTransform);
        context.DrawEllipse(Fill, BuildPen(),
            new global::Avalonia.Point(X, Y),
            Width / 2, Height / 2);
    }
}

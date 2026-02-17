using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed partial class RectangleShape : Shape
{
    public override void Render(DrawingContext context)
    {
        using var _ = context.PushTransform(RenderTransform);
        context.DrawRectangle(Fill, BuildPen(), new Rect(X, Y, Width, Height));
    }
}

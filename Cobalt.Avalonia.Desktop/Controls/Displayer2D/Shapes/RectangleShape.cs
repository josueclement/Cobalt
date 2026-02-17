using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed partial class RectangleShape : Shape
{
    public override void Render(DrawingContext context)
    {
        context.DrawRectangle(EffectiveFill, BuildPen(), new Rect(CanvasX, CanvasY, CanvasWidth, CanvasHeight));
    }
}

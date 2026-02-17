using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed partial class EllipseShape : Shape
{
    public override void Render(DrawingContext context)
    {
        context.DrawEllipse(Fill, BuildPen(),
            new global::Avalonia.Point(CanvasX + CanvasWidth / 2, CanvasY + CanvasHeight / 2),
            CanvasWidth / 2, CanvasHeight / 2);
    }
}

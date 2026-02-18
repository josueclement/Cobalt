namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

internal sealed class LineHitboxShape : RectangleShape
{
    protected override void RecalculateExtraCoordinates(double zoom, double panX, double panY)
    {
        // Width scales with zoom to cover the line length; only height is kept pixel-constant
        var centerY = CanvasY + CanvasHeight / 2;
        CanvasHeight = Height;
        CanvasY = centerY - Height / 2;
    }
}

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public class PointShape : CircleShape
{
    protected override void RecalculateExtraCoordinates(double zoom, double panX, double panY)
    {
        var centerX = CanvasX + CanvasWidth  / 2;
        var centerY = CanvasY + CanvasHeight / 2;
        CanvasWidth  = Width;
        CanvasHeight = Height;
        CanvasX = centerX - Width  / 2;
        CanvasY = centerY - Height / 2;
    }
}

using Avalonia;
using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed class PathShape : Shape
{
    public Geometry? Geometry { get; set { SetProperty(ref field, value); MarkCoordinatesDirty(); } }

    private Matrix _viewportMatrix = Matrix.Identity;
    private double _zoom = 1.0;

    protected override void RecalculateExtraCoordinates(double zoom, double panX, double panY)
    {
        _zoom = IsFixed ? 1.0 : zoom;
        _viewportMatrix = IsFixed
            ? Matrix.Identity
            : Matrix.CreateScale(zoom, zoom) * Matrix.CreateTranslation(panX, panY);
    }

    public override void Render(DrawingContext context)
    {
        if (Geometry is null) return;
        var rotation = Matrix.CreateRotation(Rotation * Math.PI / 180.0);
        var combined = rotation * Matrix.CreateTranslation(X, Y) * _viewportMatrix;
        using var _ = context.PushTransform(combined);
        // Compensate stroke thickness for zoom so it remains constant in screen pixels
        var pen = EffectiveStroke is null ? null : new Pen(EffectiveStroke, StrokeThickness / _zoom);
        context.DrawGeometry(EffectiveFill, pen, Geometry);
    }
}

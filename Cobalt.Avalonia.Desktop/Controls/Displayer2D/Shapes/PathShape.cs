using CommunityToolkit.Mvvm.ComponentModel;
using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public sealed partial class PathShape : Shape
{
    [ObservableProperty] private Geometry? _geometry;

    private Matrix _viewportMatrix = Matrix.Identity;

    protected override void RecalculateExtraCoordinates(double zoom, double panX, double panY)
    {
        _viewportMatrix = IsFixed
            ? Matrix.Identity
            : Matrix.CreateScale(zoom, zoom) * Matrix.CreateTranslation(panX, panY);
    }

    public override void Render(DrawingContext context)
    {
        if (Geometry is null) return;
        var combined = _viewportMatrix * RenderTransform * Matrix.CreateTranslation(X, Y);
        using var _ = context.PushTransform(combined);
        context.DrawGeometry(Fill, BuildPen(), Geometry);
    }
}

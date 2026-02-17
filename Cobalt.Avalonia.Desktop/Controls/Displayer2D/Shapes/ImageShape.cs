using CommunityToolkit.Mvvm.ComponentModel;
using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public partial class ImageShape : DrawingObject
{
    [ObservableProperty] private IImage? _source;

    public override void Render(DrawingContext context)
    {
        if (Source is null) return;
        var srcSize = Source.Size;
        if (srcSize.Width <= 0 || srcSize.Height <= 0) return;
        var scaleX = CanvasWidth / srcSize.Width;
        var scaleY = CanvasHeight / srcSize.Height;
        using var _ = context.PushTransform(
            Matrix.CreateScale(scaleX, scaleY) * Matrix.CreateTranslation(CanvasX, CanvasY));
        context.DrawImage(Source, new Rect(srcSize));
    }
}

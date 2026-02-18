using Avalonia;
using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public class ImageShape : DrawingObject
{
    public IImage? Source { get; set => SetProperty(ref field, value); }

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

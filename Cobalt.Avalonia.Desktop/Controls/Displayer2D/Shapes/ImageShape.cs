using CommunityToolkit.Mvvm.ComponentModel;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public partial class ImageShape : DrawingObject
{
    [ObservableProperty] private IImage? _source;

    public override void Render(DrawingContext context)
    {
        if (Source is null) return;
        context.DrawImage(Source, new global::Avalonia.Rect(CanvasX, CanvasY, CanvasWidth, CanvasHeight));
    }
}

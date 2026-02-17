using CommunityToolkit.Mvvm.ComponentModel;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public partial class ImageShape : DrawingObject
{
    [ObservableProperty] private IImage? _source;

    public override void Render(DrawingContext context)
    {
        if (Source is null) return;
        using var _ = context.PushTransform(RenderTransform);
        context.DrawImage(Source, new global::Avalonia.Rect(X, Y, Width, Height));
    }
}

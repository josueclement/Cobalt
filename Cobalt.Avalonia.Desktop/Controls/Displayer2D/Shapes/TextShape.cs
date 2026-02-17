using System.Globalization;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public class TextShape : DrawingObject
{
    public string? Text       { get; set => SetProperty(ref field, value); }
    public double  FontSize   { get; set => SetProperty(ref field, value); } = 14;
    public global::Avalonia.Media.FontFamily FontFamily { get; set => SetProperty(ref field, value); } = global::Avalonia.Media.FontFamily.Default;
    public global::Avalonia.Media.FontWeight FontWeight { get; set => SetProperty(ref field, value); } = global::Avalonia.Media.FontWeight.Normal;
    public IBrush? Foreground { get; set => SetProperty(ref field, value); }

    public override void Render(DrawingContext context)
    {
        if (string.IsNullOrEmpty(Text)) return;
        var typeface = new Typeface(FontFamily, global::Avalonia.Media.FontStyle.Normal, FontWeight);
        var ft = new FormattedText(
            Text,
            CultureInfo.CurrentCulture,
            global::Avalonia.Media.FlowDirection.LeftToRight,
            typeface,
            FontSize,
            Foreground ?? global::Avalonia.Media.Brushes.Black);
        context.DrawText(ft, new global::Avalonia.Point(CanvasX, CanvasY));
    }
}

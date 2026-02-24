using System.Globalization;
using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public class TextShape : DrawingObject
{
    public string? Text { get; set => SetProperty(ref field, value); }
    public double FontSize { get; set => SetProperty(ref field, value); } = 14;
    public FontFamily FontFamily { get; set => SetProperty(ref field, value); } = FontFamily.Default;
    public FontWeight FontWeight { get; set => SetProperty(ref field, value); } = FontWeight.Normal;
    public IBrush? Foreground { get; set => SetProperty(ref field, value); }

    public override void Render(DrawingContext context)
    {
        if (string.IsNullOrEmpty(Text)) return;
        var typeface = new Typeface(FontFamily, FontStyle.Normal, FontWeight);
        var ft = new FormattedText(
            Text,
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            typeface,
            FontSize,
            Foreground ?? Brushes.Black);
        context.DrawText(ft, new global::Avalonia.Point(CanvasX, CanvasY));
    }
}

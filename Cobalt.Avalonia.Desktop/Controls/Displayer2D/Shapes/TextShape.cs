using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public partial class TextShape : DrawingObject
{
    [ObservableProperty] private string? _text;
    [ObservableProperty] private double _fontSize = 14;
    [ObservableProperty] private global::Avalonia.Media.FontFamily _fontFamily = global::Avalonia.Media.FontFamily.Default;
    [ObservableProperty] private global::Avalonia.Media.FontWeight _fontWeight = global::Avalonia.Media.FontWeight.Normal;
    [ObservableProperty] private IBrush? _foreground;

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

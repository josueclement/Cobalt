using CommunityToolkit.Mvvm.ComponentModel;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

public abstract partial class Shape : DrawingObject
{
    [ObservableProperty] private IBrush? _fill;
    [ObservableProperty] private IBrush? _stroke;
    [ObservableProperty] private double _strokeThickness = 1.0;

    protected IPen? BuildPen() => Stroke is null ? null : new Pen(Stroke, StrokeThickness);
}

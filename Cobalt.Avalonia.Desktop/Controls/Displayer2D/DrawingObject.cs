using CommunityToolkit.Mvvm.ComponentModel;
using global::Avalonia;
using global::Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

public abstract partial class DrawingObject : ObservableObject
{
    [ObservableProperty] private double _x;
    [ObservableProperty] private double _y;
    [ObservableProperty] private int _zIndex;
    [ObservableProperty] private double _width = 100;
    [ObservableProperty] private double _height = 100;
    [ObservableProperty] private Matrix _renderTransform = Matrix.Identity;
    [ObservableProperty] private bool _isVisible = true;

    public abstract void Render(DrawingContext context);
}

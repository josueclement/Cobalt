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
    [ObservableProperty] private bool _isFixed;

    // Canvas-space coords (computed, not observable — no notification needed)
    public double CanvasX      { get; private set; }
    public double CanvasY      { get; private set; }
    public double CanvasWidth  { get; private set; }
    public double CanvasHeight { get; private set; }

    public void RecalculateCoordinates(double zoom, double panX, double panY)
    {
        if (IsFixed)
        {
            CanvasX = X; CanvasY = Y; CanvasWidth = Width; CanvasHeight = Height;
        }
        else
        {
            CanvasX      = X * zoom + panX;
            CanvasY      = Y * zoom + panY;
            CanvasWidth  = Width  * zoom;
            CanvasHeight = Height * zoom;
        }
        RecalculateExtraCoordinates(zoom, panX, panY);
    }

    protected virtual void RecalculateExtraCoordinates(double zoom, double panX, double panY) { }

    public abstract void Render(DrawingContext context);
}

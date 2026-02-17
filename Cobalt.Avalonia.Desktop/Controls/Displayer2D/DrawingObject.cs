using System;
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
    [ObservableProperty] private double _rotation;
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

    /// <summary>
    /// Pushes a rotation transform around the bounding-box centre.
    /// Returns null (and does nothing) when <see cref="Rotation"/> is zero.
    /// </summary>
    protected IDisposable? PushRotation(DrawingContext context)
    {
        if (Rotation == 0.0) return null;
        var cx  = CanvasX + CanvasWidth  / 2;
        var cy  = CanvasY + CanvasHeight / 2;
        var rad = Rotation * Math.PI / 180.0;
        var m   = Matrix.CreateTranslation(-cx, -cy)
                * Matrix.CreateRotation(rad)
                * Matrix.CreateTranslation(cx, cy);
        return context.PushTransform(m);
    }

    public abstract void Render(DrawingContext context);
}

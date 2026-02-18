using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

public abstract class DrawingObject : ObservableObject
{
    public double X      { get; set => SetProperty(ref field, value); }
    public double Y      { get; set => SetProperty(ref field, value); }
    public int    ZIndex { get; set => SetProperty(ref field, value); }
    public double Width  { get; set => SetProperty(ref field, value); } = 100;
    public double Height { get; set => SetProperty(ref field, value); } = 100;
    public double Rotation  { get; set => SetProperty(ref field, value); }
    public bool   IsVisible { get; set => SetProperty(ref field, value); } = true;
    public bool   IsFixed   { get; set => SetProperty(ref field, value); }

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

using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

public abstract class DrawingObject : ObservableObject
{
    public double X { get; set { SetProperty(ref field, value); MarkCoordinatesDirty(); } }
    public double Y { get; set { SetProperty(ref field, value); MarkCoordinatesDirty(); } }
    public int ZIndex { get; set => SetProperty(ref field, value); }
    public double Width { get; set { SetProperty(ref field, value); MarkCoordinatesDirty(); } } = 100;
    public double Height { get; set { SetProperty(ref field, value); MarkCoordinatesDirty(); } } = 100;
    public double Rotation { get; set => SetProperty(ref field, value); }
    public bool IsVisible { get; set => SetProperty(ref field, value); } = true;
    public bool IsFixed { get; set { SetProperty(ref field, value); MarkCoordinatesDirty(); } }
    public bool IsFixedWidth { get; set { SetProperty(ref field, value); MarkCoordinatesDirty(); } }
    public bool IsFixedHeight { get; set { SetProperty(ref field, value); MarkCoordinatesDirty(); } }

    // Canvas-space coords (computed, not observable — no notification needed)
    public double CanvasX { get; protected set; }
    public double CanvasY { get; protected set; }
    public double CanvasWidth { get; protected set; }
    public double CanvasHeight { get; protected set; }

    private bool _coordinatesDirty = true;
    private double _lastZoom, _lastPanX, _lastPanY;

    protected void MarkCoordinatesDirty() => _coordinatesDirty = true;

    public void RecalculateCoordinates(double zoom, double panX, double panY)
    {
        if (!_coordinatesDirty && _lastZoom == zoom && _lastPanX == panX && _lastPanY == panY)
            return;

        if (IsFixed)
        {
            CanvasX = X; CanvasY = Y; CanvasWidth = Width; CanvasHeight = Height;
        }
        else
        {
            if (IsFixedWidth)
            {
                CanvasWidth = Width;
                CanvasX = (X + Width / 2) * zoom + panX - Width / 2;
            }
            else
            {
                CanvasX = X * zoom + panX;
                CanvasWidth = Width * zoom;
            }

            if (IsFixedHeight)
            {
                CanvasHeight = Height;
                CanvasY = (Y + Height / 2) * zoom + panY - Height / 2;
            }
            else
            {
                CanvasY = Y * zoom + panY;
                CanvasHeight = Height * zoom;
            }
        }

        _coordinatesDirty = false;
        _lastZoom = zoom; _lastPanX = panX; _lastPanY = panY;

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
        var cx = CanvasX + CanvasWidth / 2;
        var cy = CanvasY + CanvasHeight / 2;
        var rad = Rotation * Math.PI / 180.0;
        var m = Matrix.CreateTranslation(-cx, -cy)
                * Matrix.CreateRotation(rad)
                * Matrix.CreateTranslation(cx, cy);
        return context.PushTransform(m);
    }

    public abstract void Render(DrawingContext context);
}

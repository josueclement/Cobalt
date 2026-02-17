using System;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

public interface IMovableDrawingObject
{
    void Move(double deltaX, double deltaY);
    event EventHandler<MovedEventArgs>? Moved;
}

public sealed class MovedEventArgs : EventArgs
{
    public double DeltaX { get; init; }
    public double DeltaY { get; init; }
    public double NewX   { get; init; }
    public double NewY   { get; init; }
}

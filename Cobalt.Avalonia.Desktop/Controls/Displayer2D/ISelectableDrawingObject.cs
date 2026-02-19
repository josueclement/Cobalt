namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

public interface ISelectableDrawingObject
{
    bool IsSelected { get; set; }
    event EventHandler<SelectionChangedEventArgs>? SelectionChanged;
}

public sealed class SelectionChangedEventArgs : EventArgs
{
    public bool IsSelected { get; init; }
}

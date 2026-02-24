namespace Cobalt.Avalonia.Desktop.Data;

public class FilterEventArgs : EventArgs
{
    public FilterEventArgs(object item)
    {
        Item = item;
    }

    public object Item { get; }

    public bool Accepted { get; set; } = true;
}

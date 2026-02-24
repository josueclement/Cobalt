namespace Cobalt.Avalonia.Desktop.Data;

public class CollectionViewGroup
{
    public CollectionViewGroup(object key, IReadOnlyList<object> items)
    {
        Key = key;
        Items = items;
    }

    public object Key { get; }

    public IReadOnlyList<object> Items { get; }

    public int ItemCount => Items.Count;
}

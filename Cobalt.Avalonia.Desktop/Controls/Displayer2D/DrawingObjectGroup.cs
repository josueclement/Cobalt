using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cobalt.Avalonia.Desktop.Controls.Displayer2D;

public abstract class DrawingObjectGroup : ObservableObject
{
    public ObservableCollection<DrawingObject> Items { get; } = new();

    protected DrawingObjectGroup()
    {
        Items.CollectionChanged += OnItemsCollectionChanged;
    }

    private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        RecalculateCoordinates();
    }

    public abstract void RecalculateCoordinates();

    public abstract void UnregisterEvents();

    protected void UnregisterCollectionEvents()
    {
        Items.CollectionChanged -= OnItemsCollectionChanged;
    }
}

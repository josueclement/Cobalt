using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using Cobalt.Avalonia.Desktop.Controls.Displayer2D.Shapes;

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
        if (e.OldItems != null)
        {
            foreach (var item in e.OldItems)
            {
                if (item is Shape shape)
                    shape.Moved -= OnItemMoved;
            }
        }

        if (e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is Shape shape)
                    shape.Moved += OnItemMoved;
            }
        }

        Console.WriteLine($"{DateTime.Now} Recalculating coordinates in DrawingObjectGroup");
        RecalculateCoordinates();
    }

    private void OnItemMoved(object? sender, MovedEventArgs e)
    {
        Console.WriteLine($"{DateTime.Now} Recalculating coordinates in DrawingObjectGroup 2");
        RecalculateCoordinates();
    }

    public abstract void RecalculateCoordinates();

    public abstract void UnregisterEvents();

    protected void UnregisterAllItemEvents()
    {
        foreach (var item in Items)
        {
            if (item is Shape shape)
                shape.Moved -= OnItemMoved;
        }
        Items.CollectionChanged -= OnItemsCollectionChanged;
    }
}

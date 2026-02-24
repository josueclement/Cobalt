using System.Collections;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Collections;

namespace Cobalt.Avalonia.Desktop.Data;

public class CollectionViewSource : AvaloniaObject
{
    public static readonly StyledProperty<IEnumerable?> SourceProperty =
        AvaloniaProperty.Register<CollectionViewSource, IEnumerable?>(nameof(Source));

    public static readonly DirectProperty<CollectionViewSource, CollectionView?> ViewProperty =
        AvaloniaProperty.RegisterDirect<CollectionViewSource, CollectionView?>(
            nameof(View),
            o => o.View);

    private CollectionView? _view;

    public CollectionViewSource()
    {
        SortDescriptions.CollectionChanged += OnSortDescriptionsCollectionChanged;
        GroupDescriptions.CollectionChanged += OnGroupDescriptionsCollectionChanged;
    }

    static CollectionViewSource()
    {
        SourceProperty.Changed.AddClassHandler<CollectionViewSource>((s, _) => s.OnSourceChanged());
    }

    public event EventHandler<FilterEventArgs>? Filter;

    public IEnumerable? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public CollectionView? View => _view;

    public AvaloniaList<SortDescription> SortDescriptions { get; } = new();

    public AvaloniaList<PropertyGroupDescription> GroupDescriptions { get; } = new();

    private void OnSourceChanged()
    {
        // Detach old view
        if (_view is not null)
        {
            _view.Detach();
            DetachDescriptionChangedHandlers(_view.SortDescriptions);
            DetachGroupDescriptionChangedHandlers(_view.GroupDescriptions);
        }

        var source = Source;
        if (source is null)
        {
            SetAndRaise(ViewProperty, ref _view, null);
            return;
        }

        var view = new CollectionView(source)
        {
            SortDescriptions = SortDescriptions,
            GroupDescriptions = GroupDescriptions,
        };

        if (Filter is not null)
        {
            view.Filter = item =>
            {
                var args = new FilterEventArgs(item);
                Filter.Invoke(this, args);
                return args.Accepted;
            };
        }

        AttachDescriptionChangedHandlers(SortDescriptions);
        AttachGroupDescriptionChangedHandlers(GroupDescriptions);

        SetAndRaise(ViewProperty, ref _view, view);
        _view!.Refresh();
    }

    private void OnSortDescriptionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (SortDescription desc in e.OldItems)
                desc.DescriptionChanged -= OnDescriptionChanged;
        }

        if (e.NewItems is not null)
        {
            foreach (SortDescription desc in e.NewItems)
                desc.DescriptionChanged += OnDescriptionChanged;
        }

        _view?.Refresh();
    }

    private void OnGroupDescriptionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (PropertyGroupDescription desc in e.OldItems)
                desc.DescriptionChanged -= OnDescriptionChanged;
        }

        if (e.NewItems is not null)
        {
            foreach (PropertyGroupDescription desc in e.NewItems)
                desc.DescriptionChanged += OnDescriptionChanged;
        }

        _view?.Refresh();
    }

    private void OnDescriptionChanged(object? sender, EventArgs e) => _view?.Refresh();

    private void AttachDescriptionChangedHandlers(AvaloniaList<SortDescription> descriptions)
    {
        foreach (var desc in descriptions)
            desc.DescriptionChanged += OnDescriptionChanged;
    }

    private void DetachDescriptionChangedHandlers(AvaloniaList<SortDescription> descriptions)
    {
        foreach (var desc in descriptions)
            desc.DescriptionChanged -= OnDescriptionChanged;
    }

    private void AttachGroupDescriptionChangedHandlers(AvaloniaList<PropertyGroupDescription> descriptions)
    {
        foreach (var desc in descriptions)
            desc.DescriptionChanged += OnDescriptionChanged;
    }

    private void DetachGroupDescriptionChangedHandlers(AvaloniaList<PropertyGroupDescription> descriptions)
    {
        foreach (var desc in descriptions)
            desc.DescriptionChanged -= OnDescriptionChanged;
    }
}

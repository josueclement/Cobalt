using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Avalonia.Collections;

namespace Cobalt.Avalonia.Desktop.Data;

public class CollectionView : IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
{
    private static readonly ConcurrentDictionary<(Type, string), Func<object, object?>> _accessorCache = new();

    private readonly IEnumerable _source;
    private List<object> _view = [];
    private IReadOnlyList<CollectionViewGroup>? _groups;
    private int _deferLevel;

    public CollectionView(IEnumerable source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));

        if (source is INotifyCollectionChanged incc)
            incc.CollectionChanged += OnSourceCollectionChanged;
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public IEnumerable SourceCollection => _source;

    public Predicate<object>? Filter { get; set; }

    public AvaloniaList<SortDescription> SortDescriptions { get; set; } = new();

    public AvaloniaList<PropertyGroupDescription> GroupDescriptions { get; set; } = new();

    public IReadOnlyList<CollectionViewGroup>? Groups => _groups;

    public int Count => _view.Count;

    public bool IsEmpty => _view.Count == 0;

    public void Refresh()
    {
        if (_deferLevel > 0)
            return;

        // 1. Filter
        var filtered = new List<object>();
        foreach (var item in _source)
        {
            if (Filter is null || Filter(item))
                filtered.Add(item);
        }

        // 2. Sort
        if (SortDescriptions.Count > 0)
        {
            filtered.Sort((a, b) =>
            {
                foreach (var desc in SortDescriptions)
                {
                    if (string.IsNullOrEmpty(desc.PropertyName))
                        continue;

                    var valA = GetPropertyValue(a, desc.PropertyName);
                    var valB = GetPropertyValue(b, desc.PropertyName);

                    int cmp = Comparer.Default.Compare(valA, valB);
                    if (cmp != 0)
                        return desc.Direction == SortDirection.Descending ? -cmp : cmp;
                }

                return 0;
            });
        }

        // 3. Group
        if (GroupDescriptions.Count > 0)
        {
            var groupDesc = GroupDescriptions[0];
            var groupDict = new Dictionary<object, List<object>>();
            var groupOrder = new List<object>();

            foreach (var item in filtered)
            {
                var key = GetGroupKey(item, groupDesc);
                if (!groupDict.TryGetValue(key, out var list))
                {
                    list = [];
                    groupDict[key] = list;
                    groupOrder.Add(key);
                }

                list.Add(item);
            }

            var groups = new List<CollectionViewGroup>(groupOrder.Count);
            foreach (var key in groupOrder)
                groups.Add(new CollectionViewGroup(key, groupDict[key].AsReadOnly()));

            _groups = groups.AsReadOnly();
        }
        else
        {
            _groups = null;
        }

        // 4. Store flat view and raise events
        _view = filtered;

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEmpty)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Groups)));
    }

    public IDisposable DeferRefresh() => new DeferToken(this);

    internal void Detach()
    {
        if (_source is INotifyCollectionChanged incc)
            incc.CollectionChanged -= OnSourceCollectionChanged;
    }

    public IEnumerator GetEnumerator() => _view.GetEnumerator();

    private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => Refresh();

    private static object? GetPropertyValue(object obj, string propertyName)
    {
        var type = obj.GetType();
        var accessor = _accessorCache.GetOrAdd((type, propertyName), static key =>
        {
            var prop = key.Item1.GetProperty(key.Item2, BindingFlags.Public | BindingFlags.Instance);
            if (prop is null)
                return _ => null;

            return o => prop.GetValue(o);
        });

        return accessor(obj);
    }

    private static object GetGroupKey(object item, PropertyGroupDescription desc)
    {
        object? rawKey = string.IsNullOrEmpty(desc.PropertyName) ? item : GetPropertyValue(item, desc.PropertyName);

        if (desc.ValueConverter is { } converter)
            rawKey = converter.Convert(rawKey, typeof(object), null!, CultureInfo.CurrentCulture);

        return rawKey ?? string.Empty;
    }

    private sealed class DeferToken : IDisposable
    {
        private CollectionView? _view;

        public DeferToken(CollectionView view)
        {
            _view = view;
            _view._deferLevel++;
        }

        public void Dispose()
        {
            if (_view is null)
                return;

            _view._deferLevel--;
            if (_view._deferLevel == 0)
                _view.Refresh();

            _view = null;
        }
    }
}

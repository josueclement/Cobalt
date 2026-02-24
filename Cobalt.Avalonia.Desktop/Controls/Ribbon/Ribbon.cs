using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Metadata;

namespace Cobalt.Avalonia.Desktop.Controls.Ribbon;

public class Ribbon : TemplatedControl
{
    private ListBox? _tabStrip;

    public Ribbon()
    {
        AttachedToVisualTree += OnAttachedToVisualTree;
    }

    [Content]
    public AvaloniaList<RibbonTab> Tabs { get; } = new();

    public static readonly StyledProperty<RibbonTab?> SelectedTabProperty =
        AvaloniaProperty.Register<Ribbon, RibbonTab?>(
            nameof(SelectedTab),
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<int> SelectedIndexProperty =
        AvaloniaProperty.Register<Ribbon, int>(
            nameof(SelectedIndex),
            defaultValue: 0,
            defaultBindingMode: BindingMode.TwoWay);

    public RibbonTab? SelectedTab
    {
        get => GetValue(SelectedTabProperty);
        set => SetValue(SelectedTabProperty, value);
    }

    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_tabStrip is not null)
            _tabStrip.SelectionChanged -= OnTabStripSelectionChanged;

        _tabStrip = e.NameScope.Find<ListBox>("PART_TabStrip");

        if (_tabStrip is not null)
        {
            _tabStrip.SelectionChanged += OnTabStripSelectionChanged;

            // Sync the ListBox selection with the current SelectedTab
            // This is important when the template is reapplied (e.g., after navigation)
            if (SelectedTab is not null)
                _tabStrip.SelectedItem = SelectedTab;
        }

        if (SelectedTab is null && Tabs.Count > 0)
            SelectedTab = Tabs[0];
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedTabProperty && _tabStrip is not null)
        {
            var tab = change.GetNewValue<RibbonTab?>();
            if (_tabStrip.SelectedItem != tab)
                _tabStrip.SelectedItem = tab;

            // Sync SelectedIndex with SelectedTab
            var newIndex = tab != null ? Tabs.IndexOf(tab) : -1;
            if (newIndex != SelectedIndex)
                SelectedIndex = newIndex;
        }
        else if (change.Property == SelectedIndexProperty)
        {
            var index = change.GetNewValue<int>();
            if (index >= 0 && index < Tabs.Count)
            {
                var tab = Tabs[index];
                if (SelectedTab != tab)
                    SelectedTab = tab;
            }
        }
    }

    private void OnTabStripSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_tabStrip?.SelectedItem is RibbonTab tab)
            SelectedTab = tab;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        // Force sync after being attached to visual tree
        // This ensures the ListBox selection is correct after navigation
        if (_tabStrip is not null && SelectedTab is not null)
        {
            // Use dispatcher to ensure bindings have been evaluated
            global::Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                if (_tabStrip.SelectedItem != SelectedTab)
                    _tabStrip.SelectedItem = SelectedTab;
            }, global::Avalonia.Threading.DispatcherPriority.Loaded);
        }
    }
}

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia;

namespace Cobalt.Avalonia.Desktop.Controls.Navigation;

public class NavigationControl : TemplatedControl
{
    public static readonly StyledProperty<IReadOnlyList<NavigationItemControl>?> ItemsProperty =
        AvaloniaProperty.Register<NavigationControl, IReadOnlyList<NavigationItemControl>?>(nameof(Items));

    public static readonly StyledProperty<IReadOnlyList<NavigationItemControl>?> FooterItemsProperty =
        AvaloniaProperty.Register<NavigationControl, IReadOnlyList<NavigationItemControl>?>(nameof(FooterItems));

    public static readonly StyledProperty<NavigationItemControl?> SelectedItemProperty =
        AvaloniaProperty.Register<NavigationControl, NavigationItemControl?>(
            nameof(SelectedItem),
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<object?> LogoProperty =
        AvaloniaProperty.Register<NavigationControl, object?>(nameof(Logo));

    private ListBox? _itemsListBox;
    private ListBox? _footerListBox;
    private bool _isSyncing;

    public IReadOnlyList<NavigationItemControl>? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public IReadOnlyList<NavigationItemControl>? FooterItems
    {
        get => GetValue(FooterItemsProperty);
        set => SetValue(FooterItemsProperty, value);
    }

    public NavigationItemControl? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public object? Logo
    {
        get => GetValue(LogoProperty);
        set => SetValue(LogoProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_itemsListBox != null)
            _itemsListBox.SelectionChanged -= OnItemsSelectionChanged;
        if (_footerListBox != null)
            _footerListBox.SelectionChanged -= OnFooterSelectionChanged;

        _itemsListBox = e.NameScope.Find<ListBox>("PART_ItemsListBox");
        _footerListBox = e.NameScope.Find<ListBox>("PART_FooterListBox");

        if (_itemsListBox != null)
            _itemsListBox.SelectionChanged += OnItemsSelectionChanged;
        if (_footerListBox != null)
            _footerListBox.SelectionChanged += OnFooterSelectionChanged;

        SyncListBoxSelection();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedItemProperty)
            SyncListBoxSelection();
    }

    private void OnItemsSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_isSyncing)
            return;

        _isSyncing = true;
        try
        {
            if (_itemsListBox?.SelectedItem is NavigationItemControl item)
            {
                if (_footerListBox != null)
                    _footerListBox.SelectedItem = null;
                SelectedItem = item;
            }
        }
        finally
        {
            _isSyncing = false;
        }
    }

    private void OnFooterSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_isSyncing)
            return;

        _isSyncing = true;
        try
        {
            if (_footerListBox?.SelectedItem is NavigationItemControl item)
            {
                if (_itemsListBox != null)
                    _itemsListBox.SelectedItem = null;
                SelectedItem = item;
            }
        }
        finally
        {
            _isSyncing = false;
        }
    }

    private void SyncListBoxSelection()
    {
        if (_isSyncing)
            return;

        _isSyncing = true;
        try
        {
            var selected = SelectedItem;

            if (selected != null && Items != null && Items.Contains(selected))
            {
                if (_itemsListBox != null)
                    _itemsListBox.SelectedItem = selected;
                if (_footerListBox != null)
                    _footerListBox.SelectedItem = null;
            }
            else if (selected != null && FooterItems != null && FooterItems.Contains(selected))
            {
                if (_itemsListBox != null)
                    _itemsListBox.SelectedItem = null;
                if (_footerListBox != null)
                    _footerListBox.SelectedItem = selected;
            }
            else
            {
                if (_itemsListBox != null)
                    _itemsListBox.SelectedItem = null;
                if (_footerListBox != null)
                    _footerListBox.SelectedItem = null;
            }
        }
        finally
        {
            _isSyncing = false;
        }
    }
}

using global::Avalonia;
using global::Avalonia.Controls;
using global::Avalonia.Controls.Presenters;
using global::Avalonia.Controls.Primitives;
using global::Avalonia.Controls.Templates;
using global::Avalonia.Data;
using global::Avalonia.Layout;

namespace Cobalt.Avalonia.Desktop.Controls.Navigation;

/// <summary>
/// A control that provides navigation capabilities, typically used for a side navigation bar.
/// </summary>
public class NavigationControl : TemplatedControl
{
    /// <summary>
    /// Defines the <see cref="Items"/> property.
    /// </summary>
    public static readonly StyledProperty<IReadOnlyList<NavigationItemControl>?> ItemsProperty =
        AvaloniaProperty.Register<NavigationControl, IReadOnlyList<NavigationItemControl>?>(nameof(Items));

    /// <summary>
    /// Defines the <see cref="FooterItems"/> property.
    /// </summary>
    public static readonly StyledProperty<IReadOnlyList<NavigationItemControl>?> FooterItemsProperty =
        AvaloniaProperty.Register<NavigationControl, IReadOnlyList<NavigationItemControl>?>(nameof(FooterItems));

    /// <summary>
    /// Defines the <see cref="SelectedItem"/> property.
    /// </summary>
    public static readonly StyledProperty<NavigationItemControl?> SelectedItemProperty =
        AvaloniaProperty.Register<NavigationControl, NavigationItemControl?>(
            nameof(SelectedItem),
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Defines the <see cref="Logo"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> LogoProperty =
        AvaloniaProperty.Register<NavigationControl, object?>(nameof(Logo));

    /// <summary>
    /// Defines the <see cref="Position"/> property.
    /// </summary>
    public static readonly StyledProperty<NavigationPosition> PositionProperty =
        AvaloniaProperty.Register<NavigationControl, NavigationPosition>(nameof(Position), NavigationPosition.Vertical);

    /// <summary>
    /// The <see cref="ListBox"/> used for main navigation items.
    /// </summary>
    private ListBox? _itemsListBox;

    /// <summary>
    /// The <see cref="ListBox"/> used for footer navigation items.
    /// </summary>
    private ListBox? _footerListBox;

    /// <summary>
    /// The <see cref="ContentPresenter"/> used for the logo.
    /// </summary>
    private ContentPresenter? _logoPresenter;

    /// <summary>
    /// Indicates whether a synchronization operation is currently in progress to avoid recursive calls.
    /// </summary>
    private bool _isSyncing;

    /// <summary>
    /// Gets or sets the main navigation items.
    /// </summary>
    public IReadOnlyList<NavigationItemControl>? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    /// <summary>
    /// Gets or sets the footer navigation items.
    /// </summary>
    public IReadOnlyList<NavigationItemControl>? FooterItems
    {
        get => GetValue(FooterItemsProperty);
        set => SetValue(FooterItemsProperty, value);
    }

    /// <summary>
    /// Gets or sets the currently selected navigation item.
    /// </summary>
    public NavigationItemControl? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    /// Gets or sets the logo content to be displayed in the navigation control.
    /// </summary>
    public object? Logo
    {
        get => GetValue(LogoProperty);
        set => SetValue(LogoProperty, value);
    }

    /// <summary>
    /// Gets or sets the position of the navigation control along the edge of a layout.
    /// </summary>
    public NavigationPosition Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
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
        _logoPresenter = e.NameScope.Find<ContentPresenter>("PART_Logo");

        if (_itemsListBox != null)
            _itemsListBox.SelectionChanged += OnItemsSelectionChanged;
        if (_footerListBox != null)
            _footerListBox.SelectionChanged += OnFooterSelectionChanged;

        SyncListBoxSelection();
        ApplyPositionLayout();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedItemProperty)
            SyncListBoxSelection();
        else if (change.Property == PositionProperty)
            ApplyPositionLayout();
    }

    /// <summary>
    /// Handles the selection change event for the main items ListBox.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the selection change event for the footer items ListBox.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Updates the layout of the navigation control based on the current <see cref="Position"/>.
    /// Sets pseudo-classes, DockPanel.Dock on template parts, and ItemsPanel orientation.
    /// </summary>
    private void ApplyPositionLayout()
    {
        bool horizontal = Position == NavigationPosition.Horizontal;

        PseudoClasses.Set(":vertical", !horizontal);
        PseudoClasses.Set(":horizontal", horizontal);

        if (_logoPresenter != null)
            DockPanel.SetDock(_logoPresenter, horizontal ? Dock.Left : Dock.Top);

        if (_footerListBox != null)
            DockPanel.SetDock(_footerListBox, horizontal ? Dock.Right : Dock.Bottom);

        var panel = CreateItemsPanel(horizontal);
        if (_itemsListBox != null)
            _itemsListBox.ItemsPanel = panel;
        if (_footerListBox != null)
            _footerListBox.ItemsPanel = panel;
    }

    /// <summary>
    /// Creates an <see cref="ItemsPanelTemplate"/> with a <see cref="StackPanel"/> of the specified orientation.
    /// </summary>
    private static FuncTemplate<Panel?> CreateItemsPanel(bool horizontal)
    {
        return new FuncTemplate<Panel?>(() => new StackPanel
        {
            Orientation = horizontal ? Orientation.Horizontal : Orientation.Vertical
        });
    }

    /// <summary>
    /// Synchronizes the selection state between the <see cref="SelectedItem"/> property and the ListBoxes.
    /// </summary>
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

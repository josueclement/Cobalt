using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Cobalt.Avalonia.Desktop.Controls.Navigation;

public class NavigationControl : TemplatedControl
{
    public static readonly StyledProperty<IReadOnlyList<NavigationItemControl>?> ItemsProperty =
        AvaloniaProperty.Register<NavigationControl, IReadOnlyList<NavigationItemControl>?>(nameof(Items));

    public static readonly StyledProperty<NavigationItemControl?> SelectedItemProperty =
        AvaloniaProperty.Register<NavigationControl, NavigationItemControl?>(
            nameof(SelectedItem),
            defaultBindingMode: BindingMode.TwoWay);

    public IReadOnlyList<NavigationItemControl>? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public NavigationItemControl? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
}

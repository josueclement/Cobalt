using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia;

namespace Cobalt.Avalonia.Desktop.Controls.Navigation;

public class NavigationItemControl : TemplatedControl
{
    public static readonly StyledProperty<string?> HeaderProperty =
        AvaloniaProperty.Register<NavigationItemControl, string?>(nameof(Header));

    public static readonly StyledProperty<Geometry?> IconDataProperty =
        AvaloniaProperty.Register<NavigationItemControl, Geometry?>(nameof(IconData));

    public static readonly StyledProperty<Func<object>?> FactoryProperty =
        AvaloniaProperty.Register<NavigationItemControl, Func<object>?>(nameof(Factory));

    public string? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public Geometry? IconData
    {
        get => GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }

    public Func<object>? Factory
    {
        get => GetValue(FactoryProperty);
        set => SetValue(FactoryProperty, value);
    }
}

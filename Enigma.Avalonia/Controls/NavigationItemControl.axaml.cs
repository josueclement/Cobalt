using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Enigma.Avalonia.ViewModels;

namespace Enigma.Avalonia.Controls;

public class NavigationItemControl : TemplatedControl
{
    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<NavigationItemControl, string>(nameof(Header), string.Empty);

    public static readonly StyledProperty<Geometry?> IconDataProperty =
        AvaloniaProperty.Register<NavigationItemControl, Geometry?>(nameof(IconData));

    public static readonly StyledProperty<Func<ViewModelBase>?> FactoryProperty =
        AvaloniaProperty.Register<NavigationItemControl, Func<ViewModelBase>?>(nameof(Factory));

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public Geometry? IconData
    {
        get => GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }

    public Func<ViewModelBase>? Factory
    {
        get => GetValue(FactoryProperty);
        set => SetValue(FactoryProperty, value);
    }
}

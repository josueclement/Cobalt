using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia;

namespace Cobalt.Avalonia.Desktop.Controls;

/// <summary>
/// A static settings card that hosts arbitrary controls (ToggleSwitch, ComboBox, Button, etc.)
/// in the right column. Unlike SettingsCardControl, this is not clickable or interactive.
/// </summary>
public class SettingsCard : TemplatedControl
{
    public static readonly StyledProperty<string?> HeaderProperty =
        AvaloniaProperty.Register<SettingsCard, string?>(nameof(Header));

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<SettingsCard, string?>(nameof(Description));

    public static readonly StyledProperty<Geometry?> IconDataProperty =
        AvaloniaProperty.Register<SettingsCard, Geometry?>(nameof(IconData));

    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<SettingsCard, object?>(nameof(Content));

    public string? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public Geometry? IconData
    {
        get => GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }

    [Content]
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
}

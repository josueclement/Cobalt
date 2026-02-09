using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Metadata;
using Avalonia.Media;

namespace Cobalt.Avalonia.Desktop.Controls;

public class SettingsCardExpander : TemplatedControl
{
    public static readonly StyledProperty<string?> HeaderProperty =
        AvaloniaProperty.Register<SettingsCardExpander, string?>(nameof(Header));

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<SettingsCardExpander, string?>(nameof(Description));

    public static readonly StyledProperty<Geometry?> IconDataProperty =
        AvaloniaProperty.Register<SettingsCardExpander, Geometry?>(nameof(IconData));

    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<SettingsCardExpander, object?>(nameof(Content));

    public static readonly StyledProperty<bool> IsExpandedProperty =
        AvaloniaProperty.Register<SettingsCardExpander, bool>(nameof(IsExpanded));

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

    public bool IsExpanded
    {
        get => GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsExpandedProperty)
        {
            if (change.GetNewValue<bool>())
                PseudoClasses.Add(":expanded");
            else
                PseudoClasses.Remove(":expanded");
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        PseudoClasses.Add(":pressed");
        IsExpanded = !IsExpanded;
        e.Handled = true;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        PseudoClasses.Remove(":pressed");
    }

    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);
        PseudoClasses.Remove(":pressed");
    }
}

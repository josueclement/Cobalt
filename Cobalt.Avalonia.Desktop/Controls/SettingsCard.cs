using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Metadata;
using System.Windows.Input;

namespace Cobalt.Avalonia.Desktop.Controls;

/// <summary>
/// A settings card that either hosts arbitrary content on the right (when Content is set)
/// or acts as a clickable card with a chevron and Command support (when Content is null).
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

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<SettingsCard, ICommand?>(nameof(Command));

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<SettingsCard, object?>(nameof(CommandParameter));

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

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ContentProperty)
        {
            if (change.NewValue is not null)
                PseudoClasses.Add(":hasContent");
            else
                PseudoClasses.Remove(":hasContent");
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (Content is not null)
            return;

        PseudoClasses.Add(":pressed");

        if (Command is { } command && command.CanExecute(CommandParameter))
        {
            command.Execute(CommandParameter);
            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (Content is null)
            PseudoClasses.Remove(":pressed");
    }

    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);

        if (Content is null)
            PseudoClasses.Remove(":pressed");
    }
}

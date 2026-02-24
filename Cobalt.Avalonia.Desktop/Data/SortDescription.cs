using Avalonia;

namespace Cobalt.Avalonia.Desktop.Data;

public class SortDescription : AvaloniaObject
{
    public static readonly StyledProperty<string?> PropertyNameProperty =
        AvaloniaProperty.Register<SortDescription, string?>(nameof(PropertyName));

    public static readonly StyledProperty<SortDirection> DirectionProperty =
        AvaloniaProperty.Register<SortDescription, SortDirection>(nameof(Direction));

    public event EventHandler? DescriptionChanged;

    static SortDescription()
    {
        PropertyNameProperty.Changed.AddClassHandler<SortDescription>((s, _) => s.OnDescriptionChanged());
        DirectionProperty.Changed.AddClassHandler<SortDescription>((s, _) => s.OnDescriptionChanged());
    }

    public string? PropertyName
    {
        get => GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    public SortDirection Direction
    {
        get => GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    private void OnDescriptionChanged() => DescriptionChanged?.Invoke(this, EventArgs.Empty);
}

using Avalonia;
using global::Avalonia.Data.Converters;

namespace Cobalt.Avalonia.Desktop.Data;

public class PropertyGroupDescription : AvaloniaObject
{
    public static readonly StyledProperty<string?> PropertyNameProperty =
        AvaloniaProperty.Register<PropertyGroupDescription, string?>(nameof(PropertyName));

    public static readonly StyledProperty<IValueConverter?> ValueConverterProperty =
        AvaloniaProperty.Register<PropertyGroupDescription, IValueConverter?>(nameof(ValueConverter));

    public event EventHandler? DescriptionChanged;

    static PropertyGroupDescription()
    {
        PropertyNameProperty.Changed.AddClassHandler<PropertyGroupDescription>((s, _) => s.OnDescriptionChanged());
        ValueConverterProperty.Changed.AddClassHandler<PropertyGroupDescription>((s, _) => s.OnDescriptionChanged());
    }

    public string? PropertyName
    {
        get => GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    public IValueConverter? ValueConverter
    {
        get => GetValue(ValueConverterProperty);
        set => SetValue(ValueConverterProperty, value);
    }

    private void OnDescriptionChanged() => DescriptionChanged?.Invoke(this, EventArgs.Empty);
}

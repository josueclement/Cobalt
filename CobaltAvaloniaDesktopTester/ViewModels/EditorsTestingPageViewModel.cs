using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public partial class EditorsTestingPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _textValue = "Hello world";

    [ObservableProperty]
    private double? _doubleValue = 3.14;

    [ObservableProperty]
    private int? _intValue = 42;

    [ObservableProperty]
    private uint? _uintValue = 42;

    [ObservableProperty]
    private short? _shortValue = -100;

    [ObservableProperty]
    private ushort? _ushortValue = 500;

    [ObservableProperty]
    private long? _longValue = 9876543210;

    [ObservableProperty]
    private ulong? _ulongValue = 1024;

    [ObservableProperty]
    private float? _singleValue = 2.718f;

    [ObservableProperty]
    private decimal? _decimalValue = 99.95m;

    [ObservableProperty]
    private string? _multiLineTextValue = "Line one\nLine two\nLine three";

    [ObservableProperty]
    private byte[]? _hexValue = [0x0A, 0xFF, 0x1B, 0x42, 0xDE, 0xAD, 0xBE, 0xEF];

    [ObservableProperty]
    private byte[]? _base64Value = Encoding.UTF8.GetBytes("Hello, Cobalt!");
}

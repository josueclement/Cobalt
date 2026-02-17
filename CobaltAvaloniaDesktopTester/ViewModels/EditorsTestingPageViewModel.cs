using System.Text;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public class EditorsTestingPageViewModel : ViewModelBase
{
    public string?  TextValue          { get; set => SetProperty(ref field, value); } = "Hello world";
    public double?  DoubleValue        { get; set => SetProperty(ref field, value); } = 3.14;
    public int?     IntValue           { get; set => SetProperty(ref field, value); } = 42;
    public uint?    UintValue          { get; set => SetProperty(ref field, value); } = 42;
    public short?   ShortValue         { get; set => SetProperty(ref field, value); } = -100;
    public ushort?  UshortValue        { get; set => SetProperty(ref field, value); } = 500;
    public long?    LongValue          { get; set => SetProperty(ref field, value); } = 9876543210;
    public ulong?   UlongValue         { get; set => SetProperty(ref field, value); } = 1024;
    public float?   SingleValue        { get; set => SetProperty(ref field, value); } = 2.718f;
    public decimal? DecimalValue       { get; set => SetProperty(ref field, value); } = 99.95m;
    public string?  MultiLineTextValue { get; set => SetProperty(ref field, value); } = "Line one\nLine two\nLine three";
    public byte[]?  HexValue           { get; set => SetProperty(ref field, value); } = [0x0A, 0xFF, 0x1B, 0x42, 0xDE, 0xAD, 0xBE, 0xEF];
    public byte[]?  Base64Value        { get; set => SetProperty(ref field, value); } = Encoding.UTF8.GetBytes("Hello, Cobalt!");
}

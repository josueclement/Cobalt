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
    private ulong? _ulongValue = 1024;
}

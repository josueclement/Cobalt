using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public partial class SettingsPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _lastAction;

    [RelayCommand]
    private void CardClicked(string? parameter)
    {
        LastAction = $"Clicked: {parameter}";
    }
}

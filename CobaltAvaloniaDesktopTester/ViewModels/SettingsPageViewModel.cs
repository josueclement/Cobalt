using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public partial class SettingsPageViewModel : ViewModelBase
{
    public string? LastAction
    {
        get;
        set => SetProperty(ref field, value);
    }

    [RelayCommand]
    private void CardClicked(string? parameter)
    {
        LastAction = $"Clicked: {parameter}";
    }
}

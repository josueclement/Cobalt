using CommunityToolkit.Mvvm.Input;
using Cobalt.Avalonia.Desktop.Services;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public partial class NavigationTestingPageViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;

    public NavigationTestingPageViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }

    [RelayCommand]
    private void NavigateToSettings()
    {
        _navigation.NavigateTo(new SettingsPageViewModel());
    }

    [RelayCommand]
    private void NavigateToDummy()
    {
        _navigation.NavigateTo(new DummyPageViewModel());
    }
}

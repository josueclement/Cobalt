using CommunityToolkit.Mvvm.Input;
using Cobalt.Avalonia.Desktop.Services;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public class NavigationTestingPageViewModel : ViewModelBase
{
    public NavigationTestingPageViewModel(NavigationService navigation)
    {
        _navigation = navigation;
        NavigateToSettingsCommand = new RelayCommand(NavigateToSettings);
        NavigateToDummyCommand = new RelayCommand(NavigateToDummy);
    }

    private readonly NavigationService _navigation;

    public IRelayCommand NavigateToSettingsCommand { get; }
    public IRelayCommand NavigateToDummyCommand { get; }

    private void NavigateToSettings()
    {
        _navigation.NavigateTo(new SettingsPageViewModel());
    }

    private void NavigateToDummy()
    {
        _navigation.NavigateTo(new DummyPageViewModel());
    }
}

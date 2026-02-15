using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Cobalt.Avalonia.Desktop;
using Cobalt.Avalonia.Desktop.Services;
using CobaltAvaloniaDesktopTester.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public class NavigationTestingPageViewModel : ViewModelBase
{
    public NavigationTestingPageViewModel(
        IServiceProvider services,
        INavigationService navigation)
    {
        _services = services;
        _navigation = navigation;
        NavigateToSettingsCommand = new AsyncRelayCommand(NavigateToSettings);
        NavigateToDummyCommand = new AsyncRelayCommand(NavigateToDummy);
    }

    private readonly IServiceProvider _services;
    private readonly INavigationService _navigation;

    public IAsyncRelayCommand NavigateToSettingsCommand { get; }
    public IAsyncRelayCommand NavigateToDummyCommand { get; }

    private async Task NavigateToSettings()
    {
        var settingsPage = _services.GetRequiredService<SettingsPageView>();
        settingsPage.DataContext = _services.GetRequiredService<SettingsPageViewModel>();
        await _navigation.NavigateToAsync(settingsPage);
    }

    private async Task NavigateToDummy()
    {
        var dummyPage = _services.GetRequiredService<DummyPageView>();
        dummyPage.DataContext = _services.GetRequiredService<DummyPageViewModel>();
        await _navigation.NavigateToAsync(dummyPage);
    }
}

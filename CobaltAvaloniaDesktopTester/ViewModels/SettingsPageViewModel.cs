using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public class SettingsPageViewModel : ViewModelBase
{
    public SettingsPageViewModel()
    {
        CardClickedCommand = new RelayCommand<string?>(CardClicked);
        ToggleThemeCommand = new RelayCommand(ToggleTheme);

        // Initialize theme state
        UpdateThemeState();
    }

    public string? LastAction
    {
        get;
        set => SetProperty(ref field, value);
    }

    public bool IsDarkTheme
    {
        get;
        set
        {
            if (SetProperty(ref field, value))
            {
                ApplyTheme(value);
            }
        }
    }

    public IRelayCommand CardClickedCommand { get; }
    public IRelayCommand ToggleThemeCommand { get; }

    private void CardClicked(string? parameter)
    {
        LastAction = $"Clicked: {parameter}";
    }

    private void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
    }

    private void ApplyTheme(bool isDark)
    {
        var app = Application.Current;
        if (app != null)
        {
            app.RequestedThemeVariant = isDark ? ThemeVariant.Dark : ThemeVariant.Light;
        }
    }

    private void UpdateThemeState()
    {
        var app = Application.Current;
        if (app != null)
        {
            IsDarkTheme = app.ActualThemeVariant == ThemeVariant.Dark;
        }
    }
}

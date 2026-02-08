using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Input;
using Cobalt.Avalonia.Desktop.Controls;
using Cobalt.Avalonia.Desktop.Services;

namespace Enigma.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public NavigationService Navigation { get; }
    public ContentDialogService DialogService { get; }
    public OverlayService OverlayService { get; }

    public MainWindowViewModel()
    {
        DialogService = new ContentDialogService();
        OverlayService = new OverlayService();

        var items = new[]
        {
            new NavigationItemControl
            {
                Header = "Keys",
                IconData = Geometry.Parse("M12.5 2a4.5 4.5 0 0 0-4.41 5.39L2 13.5V16h2.5v-2H7v-2h2l1.11-1.09A4.5 4.5 0 1 0 12.5 2Zm1.5 5a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3Z"),
                Factory = () => new GenerateKeysPageViewModel()
            },
            new NavigationItemControl
            {
                Header = "Dialogs",
                IconData = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12c0 1.73.46 3.35 1.26 4.75L2 22l5.25-1.26C8.65 21.54 10.27 22 12 22c5.52 0 10-4.48 10-10S17.52 2 12 2Zm0 18c-1.5 0-2.91-.38-4.14-1.06l-.29-.17-3.01.79.79-3.01-.17-.29A7.935 7.935 0 0 1 4 12c0-4.41 3.59-8 8-8s8 3.59 8 8-3.59 8-8 8Zm-3-9h6v2H9v-2Zm0-3h6v2H9V8Zm0 6h4v2H9v-2Z"),
                Factory = () => new ContentDialogTestingPageViewModel(DialogService)
            },
            new NavigationItemControl
            {
                Header = "Overlay",
                IconData = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2Zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8Zm-1-13h2v6h-2V7Zm0 8h2v2h-2v-2Z"),
                Factory = () => new OverlayTestingPageViewModel(OverlayService)
            },
        };

        Navigation = new NavigationService(items);
        Navigation.NavigateTo(items[0]);
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        var app = Application.Current;
        if (app != null)
        {
            app.RequestedThemeVariant = app.ActualThemeVariant == ThemeVariant.Dark
                ? ThemeVariant.Light
                : ThemeVariant.Dark;
        }
    }
}

using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Input;
using Cobalt.Avalonia.Desktop.Controls.Navigation;
using Cobalt.Avalonia.Desktop.Services;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public NavigationService Navigation { get; }
    public ContentDialogService DialogService { get; }
    public OverlayService OverlayService { get; }
    public InfoBarService InfoBarService { get; }
    public object Logo { get; }

    public MainWindowViewModel()
    {
        DialogService = new ContentDialogService();
        OverlayService = new OverlayService();
        InfoBarService = new InfoBarService();

        var items = new[]
        {
            new NavigationItemControl
            {
                Header = "Keys",
                IconData = Geometry.Parse("M12.5 2a4.5 4.5 0 0 0-4.41 5.39L2 13.5V16h2.5v-2H7v-2h2l1.11-1.09A4.5 4.5 0 1 0 12.5 2Zm1.5 5a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3Z"),
                Factory = () => new GenerateKeysPageViewModel(),
                PageType = typeof(GenerateKeysPageViewModel)
            },
            new NavigationItemControl
            {
                Header = "Dialogs",
                IconData = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12c0 1.73.46 3.35 1.26 4.75L2 22l5.25-1.26C8.65 21.54 10.27 22 12 22c5.52 0 10-4.48 10-10S17.52 2 12 2Zm0 18c-1.5 0-2.91-.38-4.14-1.06l-.29-.17-3.01.79.79-3.01-.17-.29A7.935 7.935 0 0 1 4 12c0-4.41 3.59-8 8-8s8 3.59 8 8-3.59 8-8 8Zm-3-9h6v2H9v-2Zm0-3h6v2H9V8Zm0 6h4v2H9v-2Z"),
                Factory = () => new ContentDialogTestingPageViewModel(DialogService),
                PageType = typeof(ContentDialogTestingPageViewModel)
            },
            new NavigationItemControl
            {
                Header = "Overlay",
                IconData = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2Zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8Zm-1-13h2v6h-2V7Zm0 8h2v2h-2v-2Z"),
                Factory = () => new OverlayTestingPageViewModel(OverlayService),
                PageType = typeof(OverlayTestingPageViewModel)
            },
            new NavigationItemControl
            {
                Header = "InfoBar",
                IconData = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2Zm1 15h-2v-6h2v6Zm0-8h-2V7h2v2Z"),
                Factory = () => new InfoBarTestingPageViewModel(InfoBarService),
                PageType = typeof(InfoBarTestingPageViewModel)
            },
            new NavigationItemControl
            {
                Header = "Charts",
                IconData = Geometry.Parse("M5 9.2h3V19H5V9.2ZM10.6 5h2.8v14h-2.8V5Zm5.6 8H19v6h-2.8v-6Z"),
                Factory = () => new ChartsPageViewModel(),
                PageType = typeof(ChartsPageViewModel)
            },
            new NavigationItemControl
            {
                Header = "Expander",
                IconData = Geometry.Parse("M7 14l5-5 5 5z"),
                Factory = () => new ExpanderTestingPageViewModel(),
                PageType = typeof(ExpanderTestingPageViewModel)
            },
            new NavigationItemControl
            {
                Header = "Schedule",
                IconData = Geometry.Parse("M19 4h-1V2h-2v2H8V2H6v2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2Zm0 16H5V10h14v10Zm0-12H5V6h14v2ZM9 14H7v-2h2v2Zm4 0h-2v-2h2v2Zm4 0h-2v-2h2v2Zm-8 4H7v-2h2v2Zm4 0h-2v-2h2v2Zm4 0h-2v-2h2v2Z"),
                Factory = () => new SchedulePageViewModel(),
                PageType = typeof(SchedulePageViewModel)
            },
            new NavigationItemControl
            {
                Header = "Ribbon",
                IconData = Geometry.Parse("M3 3h18v2H3V3Zm0 4h18v10H3V7Zm2 2v6h14V9H5Z"),
                Factory = () => new RibbonTestingPageViewModel(),
                PageType = typeof(RibbonTestingPageViewModel)
            },
            new NavigationItemControl
            {
                Header = "Docking",
                IconData = Geometry.Parse("M3 3h8v8H3V3Zm10 0h8v8h-8V3ZM3 13h8v8H3v-8Zm10 0h8v8h-8v-8Z"),
                Factory = () => new DockingTestingPageViewModel(),
                PageType = typeof(DockingTestingPageViewModel)
            },
        };

        var footerItems = new[]
        {
            new NavigationItemControl
            {
                Header = "Settings",
                IconData = Geometry.Parse("M19.14 12.94c.04-.3.06-.61.06-.94 0-.32-.02-.64-.07-.94l2.03-1.58a.49.49 0 0 0 .12-.61l-1.92-3.32a.49.49 0 0 0-.59-.22l-2.39.96c-.5-.38-1.03-.7-1.62-.94l-.36-2.54a.484.484 0 0 0-.48-.41h-3.84c-.24 0-.43.17-.47.41l-.36 2.54c-.59.24-1.13.57-1.62.94l-2.39-.96a.49.49 0 0 0-.59.22L2.74 8.87c-.12.21-.08.47.12.61l2.03 1.58c-.05.3-.07.62-.07.94s.02.64.07.94l-2.03 1.58a.49.49 0 0 0-.12.61l1.92 3.32c.12.22.37.29.59.22l2.39-.96c.5.38 1.03.7 1.62.94l.36 2.54c.05.24.24.41.48.41h3.84c.24 0 .44-.17.47-.41l.36-2.54c.59-.24 1.13-.56 1.62-.94l2.39.96c.22.08.47 0 .59-.22l1.92-3.32c.12-.22.07-.47-.12-.61l-2.01-1.58zM12 15.6A3.6 3.6 0 1 1 12 8.4a3.6 3.6 0 0 1 0 7.2z"),
                Factory = () => new SettingsPageViewModel(),
                PageType = typeof(SettingsPageViewModel)
            },
        };

        Logo = new Avalonia.Controls.PathIcon
        {
            Data = Geometry.Parse("M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5"),
            Width = 28,
            Height = 28,
            Foreground = new SolidColorBrush(Color.FromRgb(99, 102, 241))
        };

        Navigation = new NavigationService(items, footerItems);
        Navigation.NavigateToItem(items[0]);
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

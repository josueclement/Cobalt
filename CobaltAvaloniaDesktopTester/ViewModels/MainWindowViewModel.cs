using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using Cobalt.Avalonia.Desktop;
using CommunityToolkit.Mvvm.Input;
using Cobalt.Avalonia.Desktop.Controls.Navigation;
using Cobalt.Avalonia.Desktop.Services;
using CobaltAvaloniaDesktopTester.Views;
using Microsoft.Extensions.DependencyInjection;
using PhosphorIconsAvalonia;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _services;

    public MainWindowViewModel(
        IServiceProvider services,
        INavigationService navigation,
        IContentDialogService dialogService,
        IOverlayService overlayService,
        IInfoBarService infoBarService)
    {
        _services = services;
        Navigation = navigation;
        DialogService = dialogService;
        OverlayService = overlayService;
        InfoBarService = infoBarService;
        ToggleThemeCommand = new RelayCommand(ToggleTheme);

        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Keys",
            IconData = IconService.CreateGeometry(Icon.key, IconType.regular),
            PageType = typeof(GenerateKeysPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<GenerateKeysPageView>();
                page.DataContext = _services.GetRequiredService<GenerateKeysPageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Dialogs",
            IconData = IconService.CreateGeometry(Icon.chat_circle_text, IconType.regular),
            PageType = typeof(ContentDialogTestingPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<ContentDialogTestingPageView>();
                page.DataContext = _services.GetRequiredService<ContentDialogTestingPageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Overlay",
            IconData = IconService.CreateGeometry(Icon.check_circle, IconType.regular),
            PageType = typeof(OverlayTestingPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<OverlayTestingPageView>();
                page.DataContext = _services.GetRequiredService<OverlayTestingPageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "InfoBar",
            IconData = IconService.CreateGeometry(Icon.info, IconType.regular),
            PageType = typeof(InfoBarTestingPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<InfoBarTestingPageView>();
                page.DataContext = _services.GetRequiredService<InfoBarTestingPageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Charts",
            IconData = IconService.CreateGeometry(Icon.chart_bar, IconType.regular),
            PageType = typeof(ChartsPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<ChartsPageView>();
                page.DataContext = _services.GetRequiredService<ChartsPageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Expander",
            IconData = IconService.CreateGeometry(Icon.caret_circle_up_down, IconType.regular),
            PageType = typeof(ExpanderTestingPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<ExpanderTestingPageView>();
                page.DataContext = _services.GetRequiredService<ExpanderTestingPageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Schedule",
            IconData = IconService.CreateGeometry(Icon.calendar, IconType.regular),
            PageType = typeof(SchedulePageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<SchedulePageView>();
                page.DataContext = _services.GetRequiredService<SchedulePageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Ribbon",
            IconData = IconService.CreateGeometry(Icon.app_window, IconType.regular),
            PageType = typeof(RibbonTestingPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<RibbonTestingPageView>();
                page.DataContext = _services.GetRequiredService<RibbonTestingPageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Docking",
            IconData = IconService.CreateGeometry(Icon.square_split_horizontal, IconType.regular),
            PageType = typeof(DockingTestingPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<DockingTestingPageView>();
                page.DataContext = _services.GetRequiredService<DockingTestingPageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Navigate",
            IconData = IconService.CreateGeometry(Icon.compass, IconType.regular),
            PageType = typeof(NavigationTestingPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<NavigationTestingPageView>();
                page.DataContext = _services.GetRequiredService<NavigationTestingPageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Nav Cancel",
            IconData = IconService.CreateGeometry(Icon.file_x, IconType.regular),
            PageType = typeof(NavigationCancellationDemoPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<NavigationCancellationDemoPageView>();
                page.DataContext = _services.GetRequiredService<NavigationCancellationDemoPageViewModel>();
                return page;
            }
        });
        Navigation.Items.Add(new NavigationItemControl
        {
            Header = "Editors",
            IconData = Geometry.Parse("M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04a1 1 0 0 0 0-1.41l-2.34-2.34a1 1 0 0 0-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z"),
            PageType = typeof(EditorsTestingPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<EditorsTestingPageView>();
                page.DataContext = _services.GetRequiredService<EditorsTestingPageViewModel>();
                return page;
            }
        });

        Navigation.FooterItems.Add(new NavigationItemControl
        {
            Header = "Settings",
            IconData = IconService.CreateGeometry(Icon.gear, IconType.regular),
            PageType = typeof(SettingsPageView),
            Factory = () =>
            {
                var page = _services.GetRequiredService<SettingsPageView>();
                page.DataContext = _services.GetRequiredService<SettingsPageViewModel>();
                return page;
            }
        });

        Logo = new Avalonia.Controls.PathIcon
        {
            Data = Geometry.Parse("M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5"),
            Width = 28,
            Height = 28,
            Foreground = new SolidColorBrush(Color.FromRgb(99, 102, 241))
        };

        Navigation.NavigateToAsync(new GenerateKeysPageView { DataContext = new GenerateKeysPageViewModel() })
            .GetAwaiter().GetResult();
    }

    public INavigationService Navigation { get; }
    public IContentDialogService DialogService { get; }
    public IOverlayService OverlayService { get; }
    public IInfoBarService InfoBarService { get; }
    public object Logo { get; }

    public IRelayCommand ToggleThemeCommand { get; }

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

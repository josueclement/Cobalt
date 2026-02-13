using Cobalt.Avalonia.Desktop;
using Cobalt.Avalonia.Desktop.Services;
using CobaltAvaloniaDesktopTester.ViewModels;
using CobaltAvaloniaDesktopTester.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CobaltAvaloniaDesktopTester;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddCommonServices()
        {
            _ = services.AddSingleton<INavigationService, NavigationService>();
            _ = services.AddSingleton<IContentDialogService, ContentDialogService>();
            _ = services.AddSingleton<IInfoBarService, InfoBarService>();
            _ = services.AddSingleton<IOverlayService, OverlayService>();

            _ = services.AddSingleton<MainWindow>();
            _ = services.AddSingleton<ChartsPageView>();
            _ = services.AddSingleton<ContentDialogTestingPageView>();
            _ = services.AddSingleton<DockingTestingPageView>();
            _ = services.AddSingleton<DummyPageView>();
            _ = services.AddSingleton<EditorsTestingPageView>();
            _ = services.AddSingleton<ExpanderTestingPageView>();
            _ = services.AddSingleton<GenerateKeysPageView>();
            _ = services.AddSingleton<InfoBarTestingPageView>();
            _ = services.AddSingleton<NavigationCancellationDemoPageView>();
            _ = services.AddSingleton<NavigationTestingPageView>();
            _ = services.AddSingleton<OverlayTestingPageView>();
            _ = services.AddSingleton<RibbonTestingPageView>();
            _ = services.AddSingleton<SchedulePageView>();
            _ = services.AddSingleton<SettingsPageView>();

            _ = services.AddSingleton<MainWindowViewModel>();
            _ = services.AddSingleton<ChartsPageViewModel>();
            _ = services.AddSingleton<ContentDialogTestingPageViewModel>();
            _ = services.AddSingleton<DockingTestingPageViewModel>();
            _ = services.AddSingleton<DummyPageViewModel>();
            _ = services.AddSingleton<EditorsTestingPageViewModel>();
            _ = services.AddSingleton<ExpanderTestingPageViewModel>();
            _ = services.AddSingleton<GenerateKeysPageViewModel>();
            _ = services.AddSingleton<InfoBarTestingPageViewModel>();
            _ = services.AddSingleton<NavigationCancellationDemoPageViewModel>();
            _ = services.AddSingleton<NavigationTestingPageViewModel>();
            _ = services.AddSingleton<OverlayTestingPageViewModel>();
            _ = services.AddSingleton<RibbonTestingPageViewModel>();
            _ = services.AddSingleton<SchedulePageViewModel>();
            _ = services.AddSingleton<SettingsPageViewModel>();
        }
    }
}

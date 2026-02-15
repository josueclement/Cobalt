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
            // Views are Transient so each navigation creates a new instance
            // ViewModels remain Singleton to preserve state
            _ = services.AddTransient<ChartsPageView>();
            _ = services.AddTransient<DockingTestingPageView>();
            _ = services.AddTransient<DummyPageView>();
            _ = services.AddTransient<EditorsTestingPageView>();
            _ = services.AddTransient<NavigationDemoPageView>();
            _ = services.AddTransient<RibbonTestingPageView>();
            _ = services.AddTransient<SchedulePageView>();
            _ = services.AddTransient<ServicesTestingPageView>();
            _ = services.AddTransient<SettingsPageView>();

            _ = services.AddSingleton<MainWindowViewModel>();
            _ = services.AddSingleton<ChartsPageViewModel>();
            _ = services.AddSingleton<DockingTestingPageViewModel>();
            _ = services.AddSingleton<DummyPageViewModel>();
            _ = services.AddSingleton<EditorsTestingPageViewModel>();
            _ = services.AddSingleton<NavigationDemoPageViewModel>();
            _ = services.AddSingleton<RibbonTestingPageViewModel>();
            _ = services.AddSingleton<SchedulePageViewModel>();
            _ = services.AddSingleton<ServicesTestingPageViewModel>();
            _ = services.AddSingleton<SettingsPageViewModel>();
        }
    }
}

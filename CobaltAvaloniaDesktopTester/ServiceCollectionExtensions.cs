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
            _ = services.AddSingleton<NavigationService>();
            _ = services.AddSingleton<INavigationService>(sp => sp.GetRequiredService<NavigationService>());
            _ = services.AddSingleton<IContentDialogService, ContentDialogService>();
            _ = services.AddSingleton<IInfoBarService, InfoBarService>();
            _ = services.AddSingleton<IOverlayService, OverlayService>();

            _ = services.AddSingleton<MainWindow>();

            _ = services.AddSingleton<MainWindowViewModel>();
        }
    }
}

using Avalonia;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace CobaltAvaloniaDesktopTester;

sealed class Program
{
    internal static IHost? AppHost { get; private set; }

    [STAThread]
    public static void Main(string[] args)
    {
        // On Linux, the DBus connection disposal races with the Avalonia
        // dispatcher shutdown, throwing a TaskCanceledException on the
        // thread pool. Swallow it so the process exits cleanly.
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is TaskCanceledException ex
                && ex.StackTrace?.Contains("Tmds.DBus") == true)
                Environment.Exit(0);
        };

        AppHost = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddCobaltServices();
                services.AddPagesAndViewModels();
            })
            .Build();

        AppHost.Start();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        AppHost.StopAsync().GetAwaiter().GetResult();
        AppHost.Dispose();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}

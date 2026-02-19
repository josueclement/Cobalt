using Avalonia;
using System;
using System.Threading.Tasks;

namespace CobaltAvaloniaDesktopTester;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // On Linux, the DBus connection disposal races with the Avalonia
        // dispatcher shutdown, throwing a TaskCanceledException on the
        // thread pool. Swallow it so the process exits cleanly.
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is TaskCanceledException)
                Environment.Exit(0);
        };

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}

using System.Diagnostics;
using Cobalt.Avalonia.Desktop.Services;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public class GenerateKeysPageViewModel : ViewModelBase, INavigationLifecycle
{
    public void OnAppearing()
    {
        Debug.WriteLine("GenerateKeysPageViewModel: Page appeared");
        // Example: Load data, start timers, etc.
    }

    public void OnDisappearing()
    {
        Debug.WriteLine("GenerateKeysPageViewModel: Page disappearing");
        // Example: Clean up resources, stop timers, save state, etc.
    }
}

using System.Diagnostics;
using Avalonia.Controls;
using Cobalt.Avalonia.Desktop.Services;

namespace CobaltAvaloniaDesktopTester.Views;

public partial class GenerateKeysPageView : UserControl, INavigationLifecycle
{
    public GenerateKeysPageView()
    {
        InitializeComponent();
    }

    public void OnAppearing()
    {
        Debug.WriteLine("GenerateKeysPageView: UI appeared");
        // Example: Focus first input, start animations, etc.
    }

    public void OnDisappearing()
    {
        Debug.WriteLine("GenerateKeysPageView: UI disappearing");
        // Example: Stop animations, hide popups, etc.
    }
}

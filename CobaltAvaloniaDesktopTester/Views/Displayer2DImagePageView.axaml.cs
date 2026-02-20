using Avalonia;
using Avalonia.Controls;
using CobaltAvaloniaDesktopTester.ViewModels;

namespace CobaltAvaloniaDesktopTester.Views;

public partial class Displayer2DImagePageView : UserControl
{
    public Displayer2DImagePageView()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        Displayer.ZoomToFit();

        Displayer.PropertyChanged += OnDisplayerPropertyChanged;
    }

    private void OnDisplayerPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Cobalt.Avalonia.Desktop.Controls.Displayer2D.Displayer2DControl.WorldMousePositionProperty
            && DataContext is Displayer2DImagePageViewModel vm)
        {
            vm.UpdateCoordinates((Point?)e.NewValue);
        }
    }
}

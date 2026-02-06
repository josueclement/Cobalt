using Avalonia.Media;
using Enigma.Avalonia.Controls;
using Enigma.Avalonia.Services;

namespace Enigma.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public NavigationService Navigation { get; }

    public MainWindowViewModel()
    {
        var items = new[]
        {
            new NavigationItemControl
            {
                Header = "Keys",
                IconData = Geometry.Parse("M12.5 2a4.5 4.5 0 0 0-4.41 5.39L2 13.5V16h2.5v-2H7v-2h2l1.11-1.09A4.5 4.5 0 1 0 12.5 2Zm1.5 5a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3Z"),
                Factory = () => new GenerateKeysPageViewModel()
            },
        };

        Navigation = new NavigationService(items);
        Navigation.NavigateTo(items[0]);
    }
}

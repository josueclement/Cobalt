using Cobalt.Avalonia.Desktop.Controls.Navigation;
using Avalonia.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public interface INavigationService
{
    object? CurrentPage { get; }
    NavigationItemControl? SelectedItem { get; set; }
    IReadOnlyList<NavigationItemControl> Items { get; }
    IReadOnlyList<NavigationItemControl>? FooterItems { get; }
    void NavigateToItem(NavigationItemControl item);
    Task NavigateToAsync(Control page);
    Task NavigateToItemAsync(int index);
}

using Cobalt.Avalonia.Desktop.Controls.Navigation;

namespace Cobalt.Avalonia.Desktop.Services;

public interface INavigationService
{
    object? CurrentPage { get; }
    NavigationItemControl? SelectedItem { get; set; }
    IReadOnlyList<NavigationItemControl> Items { get; }
    IReadOnlyList<NavigationItemControl>? FooterItems { get; }
    void NavigateTo(object page);
    void NavigateToItem(NavigationItemControl item);
    Task NavigateToAsync(object page);
    Task NavigateToItemAsync(int index);
}

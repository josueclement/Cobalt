using Cobalt.Avalonia.Desktop.Controls.Navigation;

namespace Cobalt.Avalonia.Desktop.Services;

public interface INavigationService
{
    object? CurrentPage { get; }
    NavigationItemControl? SelectedItem { get; set; }
    IReadOnlyList<NavigationItemControl> Items { get; }
    void NavigateTo(NavigationItemControl item);
    Task NavigateToAsync(NavigationItemControl item);
    Task NavigateToAsync(int index);
}

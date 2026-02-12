using Avalonia.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public interface INavigationService
{
    Control? CurrentPage { get; }
    Task NavigateToAsync(Control page);
}

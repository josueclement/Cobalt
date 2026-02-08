using System.Collections.Generic;
using Cobalt.Avalonia.Desktop.Controls;
using Enigma.Avalonia.ViewModels;

namespace Enigma.Avalonia.Services;

public interface INavigationService
{
    ViewModelBase? CurrentPage { get; }
    NavigationItemControl? SelectedItem { get; set; }
    IReadOnlyList<NavigationItemControl> Items { get; }
    void NavigateTo(NavigationItemControl item);
}

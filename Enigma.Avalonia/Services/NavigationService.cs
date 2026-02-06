using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Enigma.Avalonia.Controls;
using Enigma.Avalonia.ViewModels;

namespace Enigma.Avalonia.Services;

public partial class NavigationService : ObservableObject, INavigationService
{
    [ObservableProperty]
    private ViewModelBase? _currentPage;

    [ObservableProperty]
    private NavigationItemControl? _selectedItem;

    public IReadOnlyList<NavigationItemControl> Items { get; }

    public NavigationService(IReadOnlyList<NavigationItemControl> items)
    {
        Items = items;
    }

    public void NavigateTo(NavigationItemControl item)
    {
        SelectedItem = item;
    }

    partial void OnSelectedItemChanged(NavigationItemControl? value)
    {
        CurrentPage = value?.Factory?.Invoke();
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cobalt.Avalonia.Desktop.Controls.Navigation;

namespace Cobalt.Avalonia.Desktop.Services;

public class NavigationService : INavigationService, INotifyPropertyChanged
{
    private object? _currentPage;
    private NavigationItemControl? _selectedItem;

    public event PropertyChangedEventHandler? PropertyChanged;

    public object? CurrentPage
    {
        get => _currentPage;
        private set
        {
            if (_currentPage != value)
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }
    }

    public NavigationItemControl? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged();
                CurrentPage = value?.Factory?.Invoke();
            }
        }
    }

    public IReadOnlyList<NavigationItemControl> Items { get; }

    public NavigationService(IReadOnlyList<NavigationItemControl> items)
    {
        Items = items;
    }

    public void NavigateTo(NavigationItemControl item) => SelectedItem = item;

    public Task NavigateToAsync(NavigationItemControl item)
    {
        NavigateTo(item);
        return Task.CompletedTask;
    }

    public Task NavigateToAsync(int index)
    {
        NavigateTo(Items[index]);
        return Task.CompletedTask;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

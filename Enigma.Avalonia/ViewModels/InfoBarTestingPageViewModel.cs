using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cobalt.Avalonia.Desktop.Controls;
using Cobalt.Avalonia.Desktop.Services;

namespace Enigma.Avalonia.ViewModels;

public partial class InfoBarTestingPageViewModel : ViewModelBase
{
    private readonly IInfoBarService _infoBarService;

    [ObservableProperty]
    private string? _lastResult;

    public InfoBarTestingPageViewModel(IInfoBarService infoBarService)
    {
        _infoBarService = infoBarService;
    }

    [RelayCommand]
    private async Task ShowInfo()
    {
        await _infoBarService.ShowAsync(o =>
        {
            o.Title = "Information";
            o.Message = "This is an informational message.";
            o.Severity = InfoBarSeverity.Info;
        });
        LastResult = "Info closed";
    }

    [RelayCommand]
    private async Task ShowSuccess()
    {
        await _infoBarService.ShowAsync(o =>
        {
            o.Title = "Success";
            o.Message = "The operation completed successfully.";
            o.Severity = InfoBarSeverity.Success;
        });
        LastResult = "Success closed";
    }

    [RelayCommand]
    private async Task ShowWarning()
    {
        await _infoBarService.ShowAsync(o =>
        {
            o.Title = "Warning";
            o.Message = "Something might need your attention.";
            o.Severity = InfoBarSeverity.Warning;
        });
        LastResult = "Warning closed";
    }

    [RelayCommand]
    private async Task ShowError()
    {
        await _infoBarService.ShowAsync(o =>
        {
            o.Title = "Error";
            o.Message = "An error has occurred during the operation.";
            o.Severity = InfoBarSeverity.Error;
        });
        LastResult = "Error closed";
    }

    [RelayCommand]
    private void Close()
    {
        _infoBarService.Hide();
    }
}

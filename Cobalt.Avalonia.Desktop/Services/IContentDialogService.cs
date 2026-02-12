using Cobalt.Avalonia.Desktop.Controls;

namespace Cobalt.Avalonia.Desktop.Services;

public interface IContentDialogService
{
    // TODO: Should there be a show message dialog?
    Task<DialogResult> ShowMessageAsync(string title, string message, string closeButtonText = "OK");
    Task<DialogResult> ShowAsync(Action<ContentDialog> configure);
    Task HideAsync();
}

using Avalonia.Platform.Storage;

namespace Cobalt.Avalonia.Desktop.Services;

public interface IFileDialogService
{
    IStorageProvider? StorageProvider { get; }
    void SetStorageProvider(IStorageProvider storageProvider);
    Task<IReadOnlyList<IStorageFile>> ShowOpenFileDialogAsync(FilePickerOpenOptions options);
    Task<IStorageFile?> ShowSaveFileDialogAsync(FilePickerSaveOptions options);
}
using Avalonia.Platform.Storage;

namespace Cobalt.Avalonia.Desktop.Services;

public interface IFolderDialogService
{
    IStorageProvider? StorageProvider { get; }
    void SetStorageProvider(IStorageProvider storageProvider);
    Task<IReadOnlyList<IStorageFolder>> ShowOpenFolderDialogAsync(FolderPickerOpenOptions options);
}
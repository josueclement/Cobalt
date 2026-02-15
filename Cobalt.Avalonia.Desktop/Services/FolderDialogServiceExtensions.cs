using Avalonia.Platform.Storage;

namespace Cobalt.Avalonia.Desktop.Services;

public static class FolderDialogServiceExtensions
{
    extension(IFolderDialogService service)
    {
        public async Task<IEnumerable<string>> ShowOpenFolderDialogAsync(
            string? title = null,
            bool allowMultiple = false,
            string? suggestedStartLocation = null,
            string? suggestedFileName = null)
        {
            if (service.StorageProvider is null)
                throw new InvalidOperationException("Storage provider is not set");
        
            var options = new FolderPickerOpenOptions();
        
            if (title is not null)
                options.Title = title;
            options.AllowMultiple = allowMultiple;
            if (suggestedStartLocation is not null)
                options.SuggestedStartLocation =
                    await service.StorageProvider.TryGetFolderFromPathAsync(suggestedStartLocation);
            if (suggestedFileName is not null)
                options.SuggestedFileName = suggestedFileName;
        
            var folders = await service.ShowOpenFolderDialogAsync(options);
        
            List<string> paths = [];
            paths.AddRange(folders.Select(folder => folder.TryGetLocalPath()).OfType<string>());

            return paths; 
        }
    }
}
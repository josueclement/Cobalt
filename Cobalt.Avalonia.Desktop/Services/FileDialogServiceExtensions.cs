using Avalonia.Platform.Storage;

namespace Cobalt.Avalonia.Desktop.Services;

public static class FileDialogServiceExtensions
{
    extension(IFileDialogService service)
    {
        public async Task<IEnumerable<string>> ShowOpenFileDialogAsync(
            string? title = null,
            bool allowMultiple = false,
            string? suggestedStartLocation = null,
            string? suggestedFileName = null,
            IReadOnlyList<FilePickerFileType>? fileTypeFilter = null)
        {
            if (service.StorageProvider is null)
                throw new InvalidOperationException("Storage provider is not set");

            var options = new FilePickerOpenOptions();

            if (title is not null)
                options.Title = title;
            options.AllowMultiple = allowMultiple;
            if (suggestedStartLocation is not null)
                options.SuggestedStartLocation =
                    await service.StorageProvider.TryGetFolderFromPathAsync(suggestedStartLocation);
            if (suggestedFileName is not null)
                options.SuggestedFileName = suggestedFileName;
            options.FileTypeFilter = fileTypeFilter;

            var files = await service.ShowOpenFileDialogAsync(options);

            List<string> paths = [];
            paths.AddRange(files.Select(folder => folder.TryGetLocalPath()).OfType<string>());

            return paths;
        }

        public async Task<string?> ShowSaveFileDialogAsync(
            string? title = null,
            string? suggestedStartLocation = null,
            string? suggestedFileName = null,
            string? defaultExtension = null,
            bool showOverwritePrompt = true,
            IReadOnlyList<FilePickerFileType>? fileTypeChoices = null)
        {
            if (service.StorageProvider is null)
                throw new InvalidOperationException("Storage provider is not set");
        
            var options = new FilePickerSaveOptions();
        
            if (title is not null)
                options.Title = title;
            if (suggestedStartLocation is not null)
                options.SuggestedStartLocation =
                    await service.StorageProvider.TryGetFolderFromPathAsync(suggestedStartLocation);
            if (suggestedFileName is not null)
                options.SuggestedFileName = suggestedFileName;
            if (defaultExtension is not null)
                options.DefaultExtension = defaultExtension;
            options.ShowOverwritePrompt = showOverwritePrompt;
            if (fileTypeChoices is not null)
                options.FileTypeChoices = fileTypeChoices;
        
            var file = await service.ShowSaveFileDialogAsync(options);
            return file?.TryGetLocalPath();
        }
    }
}
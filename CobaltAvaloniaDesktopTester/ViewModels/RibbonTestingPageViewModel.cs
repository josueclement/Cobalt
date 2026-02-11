using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CobaltAvaloniaDesktopTester.ViewModels;

public partial class RibbonTestingPageViewModel : ViewModelBase
{
    public string StatusText
    {
        get;
        set => SetProperty(ref field, value);
    } = "Ready";

    public bool IsBoldActive
    {
        get;
        set => SetProperty(ref field, value);
    }

    public bool IsItalicActive
    {
        get;
        set => SetProperty(ref field, value);
    }

    [RelayCommand]
    private void New() => StatusText = "New file created";

    [RelayCommand]
    private void Open() => StatusText = "Open file dialog";

    [RelayCommand]
    private void Save() => StatusText = "File saved";

    [RelayCommand]
    private void Cut() => StatusText = "Cut to clipboard";

    [RelayCommand]
    private void Copy() => StatusText = "Copied to clipboard";

    [RelayCommand]
    private void Paste() => StatusText = "Pasted from clipboard";

    [RelayCommand]
    private void Undo() => StatusText = "Undo";

    [RelayCommand]
    private void Redo() => StatusText = "Redo";

    [RelayCommand]
    private void ToggleBold() => StatusText = IsBoldActive ? "Bold enabled" : "Bold disabled";

    [RelayCommand]
    private void ToggleItalic() => StatusText = IsItalicActive ? "Italic enabled" : "Italic disabled";

    [RelayCommand]
    private void InsertImage() => StatusText = "Insert image";

    [RelayCommand]
    private void InsertTable() => StatusText = "Insert table";

    [RelayCommand]
    private void InsertLink() => StatusText = "Insert link";

    [RelayCommand]
    private void ExportPdf() => StatusText = "Exported as PDF";

    [RelayCommand]
    private void ExportCsv() => StatusText = "Exported as CSV";

    [RelayCommand]
    private void ExportJson() => StatusText = "Exported as JSON";
}

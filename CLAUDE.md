# Enigma.Avalonia

Avalonia desktop GUI for the [Enigma.Cryptography](https://github.com/josueclement/Enigma) library.

## Build & Run

```bash
dotnet build          # Build the project
dotnet run            # Run the application
```

## Architecture

- **Framework**: Avalonia 11.3 with FluentTheme (Dark variant), .NET 10.0
- **MVVM**: CommunityToolkit.Mvvm with compiled bindings
- **Crypto**: Enigma.Cryptography (BouncyCastle-based)

### Project Structure

```
├── Models/              # Data models
├── Services/            # Services (NavigationService)
├── Controls/            # Control classes (NavigationControl, NavigationItemControl)
├── Styling/             # Control styles and templates (XAML)
├── ViewModels/          # ViewModels (ViewModelBase, MainWindowViewModel, page VMs)
├── Views/               # Views (MainWindow, page views)
├── App.axaml            # Application root, theme config
├── ViewLocator.cs       # Convention-based ViewModel→View resolution
└── Program.cs           # Entry point
```

### Patterns

- **ViewLocator**: Resolves views by replacing "ViewModel" with "View" in the type name. Page VMs go in `ViewModels/`, matching views in `Views/`.
- **Navigation**:
  - `NavigationControl` is a templated `Control` (not `UserControl`) that displays navigation items
  - `NavigationItemControl` is a templated `Control` with properties: `Header` (string), `IconData` (Geometry), `Factory` (Func<ViewModelBase>)
  - `NavigationService` holds a list of `NavigationItemControl` instances. Selecting an item invokes its factory to create the page ViewModel, which `ContentControl` + `ViewLocator` renders.
- **Naming**: Page ViewModels are `{Name}PageViewModel`, views are `{Name}PageView`.

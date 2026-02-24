# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

**Cobalt.Avalonia.Desktop** is a reusable Avalonia control library. **CobaltAvaloniaDesktopTester** is the companion WinExe app used to develop and test the controls.

## Build & Run

```bash
dotnet build                                        # Build the solution (both projects)
dotnet run --project CobaltAvaloniaDesktopTester     # Run the tester app
```

No test projects exist yet.

## Architecture

- **Framework**: Avalonia 11.3 with FluentTheme (Dark variant), .NET 10.0
- **MVVM**: CommunityToolkit.Mvvm with compiled bindings (`AvaloniaUseCompiledBindingsByDefault=true`)
- **Icons**: PhosphorIconsAvalonia — use `IconService.CreateGeometry(Icon.xxx, IconType.regular)` for navigation item icons

### Two-Project Structure

**CobaltAvaloniaDesktopTester** — Tester application (WinExe). Contains ViewModels, Views, and app entry point. Depends on CommunityToolkit.Mvvm, Enigma.Cryptography, LiveChartsCore, PhosphorIconsAvalonia.

**Cobalt.Avalonia.Desktop** — Reusable control library (no CommunityToolkit dependency). Contains:
- `Controls/` — TemplatedControls organized in subdirectories: `Navigation/`, `Docking/`, `Ribbon/`, `Editors/`, `CalendarSchedule/`, `Displayer2D/`, plus top-level `ContentDialog`, `Overlay`, `InfoBar`, `SettingsCard`, `SettingsCardExpander`
- `Services/` — Each service control has a matching service + interface: `NavigationService`, `ContentDialogService`, `OverlayService`, `InfoBarService`
- `Themes/` — Control templates (AXAML). Composed via `Fluent.axaml` and included in App.axaml as `avares://Cobalt.Avalonia.Desktop/Themes/Fluent.axaml`

### Patterns

- **ViewLocator**: Resolves views by replacing "ViewModel" with "View" in the type name. Page VMs go in `ViewModels/`, matching views in `Views/`. ViewModels inherit from `ObservableObject` directly (no `ViewModelBase`).
- **Naming**: Page ViewModels are `{Name}PageViewModel`, views are `{Name}PageView`.
- **Navigation**: `NavigationService` holds `NavigationItem` instances. Selecting an item invokes its `Factory` (`Func<object>`) to create a page ViewModel, which `ContentControl` + `ViewLocator` renders.
- **Host pattern**: `ContentDialog`, `Overlay`, and `InfoBar` are all hosted as siblings in MainWindow's root `Panel`. Each is registered with its service via `RegisterHost()` from `App.axaml.cs` `OnFrameworkInitializationCompleted`. ViewModels receive services through constructor injection from `MainWindowViewModel`.
- **TemplatedControls**: Custom controls in Cobalt are `TemplatedControl` (not `UserControl`). C# classes go in `Controls/`, AXAML templates in `Themes/`. When adding a new control, also add its template include to `Themes/Fluent.axaml`. Use `{Binding ..., RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}` for two-way template bindings — `{TemplateBinding}` is one-way only in Avalonia.
- **OnApplyTemplate pattern**: Controls find named template parts via `e.NameScope.Find<T>("PART_Name")` in `OnApplyTemplate`. Always detach old event handlers before attaching new ones, since `OnApplyTemplate` can be called multiple times.

### Theme System

Colors are defined in `Cobalt.Avalonia.Desktop/Themes/Colors.axaml` using `ResourceDictionary.ThemeDictionaries` with `x:Key="Dark"` and `x:Key="Light"` variants. Brushes in `Brushes.axaml` reference colors via `{DynamicResource}` for runtime theme switching. Use `{DynamicResource CobaltForegroundSecondaryBrush}` (not `StaticResource`) for theme-reactive styling. Key prefixes: `CobaltBackground`, `CobaltSurface`, `CobaltBorder`, `CobaltForeground`, `CobaltAccent`, `CobaltSuccess`, `CobaltWarning`, `CobaltError`.

### Property & Command conventions

- **Observable properties**: Use C# 13 semi-auto properties with `SetProperty`. Never use `[ObservableProperty]`. Classes must NOT be `partial`.
  ```csharp
  public string Text { get; set => SetProperty(ref field, value); } = "default";
  ```
- **Commands**: Declare as `IRelayCommand` / `IAsyncRelayCommand` properties, initialize in the constructor with `new RelayCommand(Method)` / `new AsyncRelayCommand(AsyncMethod)`. Never use `[RelayCommand]`.
  ```csharp
  public IRelayCommand SaveCommand { get; }
  // in constructor:
  SaveCommand = new RelayCommand(Save);
  ```

### Gotchas

- **Namespace collision**: The `Cobalt.Avalonia.Desktop` namespace causes `Avalonia.Media` (and similar) to resolve incorrectly as `Cobalt.Avalonia.Media`. Use `global::Avalonia.Media` or `using global::Avalonia.Media;` in C# files that need Avalonia sub-namespaces, BUT ONLY WHEN ABSOLUTELY NECESSARY.
- **Compiled bindings require `x:DataType`**: Every UserControl/DataTemplate using `{Binding}` needs an explicit `x:DataType` or the build fails with AVLN2100.
- **Hit testing requires a Background**: A `Border` or `Panel` with no `Background` (null) is invisible to pointer events. Set `Background="Transparent"` on elements that need to receive clicks/hover across their full area.

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Avalonia desktop GUI for the [Enigma.Cryptography](https://github.com/josueclement/Enigma) library.

## Build & Run

```bash
dotnet build          # Build the solution (both projects)
dotnet run            # Run from Enigma.Avalonia/ directory
```

No test projects exist yet.

## Architecture

- **Framework**: Avalonia 11.3 with FluentTheme (Dark variant), .NET 10.0
- **MVVM**: CommunityToolkit.Mvvm with compiled bindings (`AvaloniaUseCompiledBindingsByDefault=true`)
- **Crypto**: Enigma.Cryptography (BouncyCastle-based)

### Two-Project Structure

**Enigma.Avalonia** — Main application (WinExe). Contains ViewModels, Views, and app entry point.

**Cobalt.Avalonia.Desktop** — Reusable control library (no CommunityToolkit dependency). Contains:
- `Controls/` — TemplatedControls: `NavigationControl`, `NavigationItemControl`, `ContentDialog`, `OverlayControl`, `InfoBarControl`, `SettingsCardControl`, `SettingsCardExpander`, `CalendarScheduleControl`
- `Services/` — Each service control has a matching service + interface: `NavigationService`, `ContentDialogService`, `OverlayService`, `InfoBarService`
- `Themes/` — Control templates (AXAML). Composed via `Fluent.axaml` and included in App.axaml as `avares://Cobalt.Avalonia.Desktop/Themes/Fluent.axaml`

### Patterns

- **ViewLocator**: Resolves views by replacing "ViewModel" with "View" in the type name. Page VMs go in `ViewModels/`, matching views in `Views/`. ViewModels must inherit from `ViewModelBase` (`ObservableObject`) to be matched.
- **Naming**: Page ViewModels are `{Name}PageViewModel`, views are `{Name}PageView`.
- **Navigation**: `NavigationService` holds `NavigationItemControl` instances. Selecting an item invokes its `Factory` (`Func<object>`) to create a page ViewModel, which `ContentControl` + `ViewLocator` renders.
- **Host pattern**: `ContentDialog`, `OverlayControl`, and `InfoBarControl` are all hosted as siblings in MainWindow's root `Panel`. Each is registered with its service via `RegisterHost()` from `MainWindow.axaml.cs` `OnDataContextChanged`. ViewModels receive services through constructor injection from `MainWindowViewModel`.
- **TemplatedControls**: Custom controls in Cobalt are `TemplatedControl` (not `UserControl`). C# classes go in `Controls/`, AXAML templates in `Themes/`. When adding a new control, also add its template include to `Themes/Fluent.axaml`. Use `{Binding ..., RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}` for two-way template bindings — `{TemplateBinding}` is one-way only in Avalonia.
- **OnApplyTemplate pattern**: Controls find named template parts via `e.NameScope.Find<T>("PART_Name")` in `OnApplyTemplate`. Always detach old event handlers before attaching new ones, since `OnApplyTemplate` can be called multiple times.

### Theme System

Colors are defined in `Cobalt.Avalonia.Desktop/Themes/Colors.axaml` using `ResourceDictionary.ThemeDictionaries` with `x:Key="Dark"` and `x:Key="Light"` variants. Brushes in `Brushes.axaml` reference colors via `{DynamicResource}` for runtime theme switching. Use `{DynamicResource CobaltForegroundSecondaryBrush}` (not `StaticResource`) for theme-reactive styling. Key prefixes: `CobaltBackground`, `CobaltSurface`, `CobaltBorder`, `CobaltForeground`, `CobaltAccent`, `CobaltSuccess`, `CobaltWarning`, `CobaltError`.

### Gotchas

- **Namespace collision**: Both `Enigma.Avalonia` and `Cobalt.Avalonia.Desktop` namespaces cause `Avalonia.Media` (and similar) to resolve incorrectly. Use `global::Avalonia.Media` or `using global::Avalonia.Media;` in C# files that need Avalonia sub-namespaces.
- **Compiled bindings require `x:DataType`**: Every UserControl/DataTemplate using `{Binding}` needs an explicit `x:DataType` or the build fails with AVLN2100.
- **Hit testing requires a Background**: A `Border` or `Panel` with no `Background` (null) is invisible to pointer events. Set `Background="Transparent"` on elements that need to receive clicks/hover across their full area.

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Avalonia desktop GUI for the [Enigma.Cryptography](https://github.com/josueclement/Enigma) library.

## Build & Run

```bash
dotnet build          # Build the solution (both projects)
dotnet run            # Run from Enigma.Avalonia/ directory
```

## Architecture

- **Framework**: Avalonia 11.3 with FluentTheme (Dark variant), .NET 10.0
- **MVVM**: CommunityToolkit.Mvvm with compiled bindings (`AvaloniaUseCompiledBindingsByDefault=true`)
- **Crypto**: Enigma.Cryptography (BouncyCastle-based)

### Two-Project Structure

**Enigma.Avalonia** — Main application (WinExe). Contains ViewModels, Views, and app entry point.

**Cobalt.Avalonia.Desktop** — Reusable control library (no CommunityToolkit dependency). Contains:
- `Controls/` — TemplatedControls: `NavigationControl`, `NavigationItemControl`, `ContentDialog`
- `Services/` — `NavigationService` (INotifyPropertyChanged), `ContentDialogService`
- `Themes/` — Control templates (AXAML). Composed via `Fluent.axaml` and included in App.axaml as `avares://Cobalt.Avalonia.Desktop/Themes/Fluent.axaml`

### Patterns

- **ViewLocator**: Resolves views by replacing "ViewModel" with "View" in the type name. Page VMs go in `ViewModels/`, matching views in `Views/`.
- **Naming**: Page ViewModels are `{Name}PageViewModel`, views are `{Name}PageView`.
- **Navigation**: `NavigationService` holds `NavigationItemControl` instances. Selecting an item invokes its `Factory` (`Func<object>`) to create a page ViewModel, which `ContentControl` + `ViewLocator` renders.
- **ContentDialog hosting**: MainWindow wraps its content in a `Panel` with `<ContentDialog x:Name="HostDialog"/>` as the last child. The host is registered via `OnDataContextChanged` so ViewModels can show dialogs through `ContentDialogService`.
- **TemplatedControls**: Custom controls in Cobalt are `TemplatedControl` (not `UserControl`). C# classes go in `Controls/`, AXAML templates in `Themes/`. Use `{Binding ..., RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}` for two-way template bindings — `{TemplateBinding}` is one-way only in Avalonia.

### Gotchas

- **Namespace collision**: Both `Enigma.Avalonia` and `Cobalt.Avalonia.Desktop` namespaces cause `Avalonia.Media` (and similar) to resolve incorrectly. Use `global::Avalonia.Media` or `using global::Avalonia.Media;` in C# files that need Avalonia sub-namespaces.
- **Compiled bindings require `x:DataType`**: Every UserControl/DataTemplate using `{Binding}` needs an explicit `x:DataType` or the build fails with AVLN2100.

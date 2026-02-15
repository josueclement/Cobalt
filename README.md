# Cobalt.Avalonia.Desktop

A comprehensive, reusable Avalonia UI control library featuring modern navigation, docking, ribbon, editors, and service-based components for desktop applications.

## Features

- **Navigation System** - Flexible navigation controls with hierarchical menu support
- **Docking System** - Advanced docking panel layout with drag-and-drop support
- **Ribbon Controls** - Modern ribbon UI components for command organization
- **Editor Controls** - Rich text and code editing capabilities
- **Calendar & Schedule** - Calendar and scheduling components
- **Service-Based Components** - ContentDialog, Overlay, and InfoBar with service pattern
- **Settings Controls** - SettingsCard and SettingsCardExpander for configuration UIs
- **Dark/Light Theme Support** - Complete theming system with dynamic resource switching
- **Compiled Bindings** - Performance-optimized with compiled bindings by default

## Requirements

- .NET 10.0 or later
- Avalonia 11.3+
- CommunityToolkit.Mvvm 8.4+ (for ViewModels in your application)

## Installation

### 1. Add Package Reference

Add the library to your project:

```xml
<ItemGroup>
  <ProjectReference Include="..\Cobalt.Avalonia.Desktop\Cobalt.Avalonia.Desktop.csproj" />
</ItemGroup>
```

*(Or add as NuGet package once published)*

### 2. Include Theme Resources

In your `App.axaml`, include the Cobalt theme resources:

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="YourApp.App">
  <Application.Styles>
    <FluentTheme />
    <StyleInclude Source="avares://Cobalt.Avalonia.Desktop/Themes/Fluent.axaml" />
  </Application.Styles>
</Application>
```

### 3. Configure Services in App.axaml.cs

Set up dependency injection and register Cobalt services in your `App.axaml.cs`:

```csharp
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Cobalt.Avalonia.Desktop.Services;
using Microsoft.Extensions.DependencyInjection;

namespace YourApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Create service collection
        var services = new ServiceCollection();

        // Register Cobalt services
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IContentDialogService, ContentDialogService>();
        services.AddSingleton<IInfoBarService, InfoBarService>();
        services.AddSingleton<IOverlayService, OverlayService>();

        // Register your main window and view models
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();

        // Register your views as Transient (new instance per navigation)
        services.AddTransient<HomePageView>();
        services.AddTransient<SettingsPageView>();
        // ... other views

        // Register your view models as Singleton (preserve state)
        services.AddSingleton<HomePageViewModel>();
        services.AddSingleton<SettingsPageViewModel>();
        // ... other view models

        // Build service provider
        var serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            var viewModel = serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindow.DataContext = viewModel;

            // Register service hosts (if using ContentDialog, Overlay, or InfoBar)
            serviceProvider.GetRequiredService<IContentDialogService>()
                .RegisterHost(mainWindow.HostDialog);
            serviceProvider.GetRequiredService<IOverlayService>()
                .RegisterHost(mainWindow.HostOverlay);
            serviceProvider.GetRequiredService<IInfoBarService>()
                .RegisterHost(mainWindow.HostInfoBar);

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
```

### 4. Set Up Host Controls in MainWindow

For services like ContentDialog, Overlay, and InfoBar, add host controls to your `MainWindow.axaml`:

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:controls="using:Cobalt.Avalonia.Desktop.Controls"
        x:Class="YourApp.MainWindow">

  <Panel>
    <!-- Your main content -->
    <ContentControl Content="{Binding CurrentPage}" />

    <!-- Service host controls (siblings at root level) -->
    <controls:ContentDialog x:Name="HostDialog" />
    <controls:OverlayControl x:Name="HostOverlay" />
    <controls:InfoBarControl x:Name="HostInfoBar" />
  </Panel>
</Window>
```

## Usage Examples

### Navigation Service

```csharp
public class MainWindowViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        // Add navigation items
        _navigationService.Items.Add(new NavigationItemControl
        {
            Header = "Home",
            Icon = IconService.CreateGeometry(Icon.House, IconType.regular),
            Factory = () => App.Current.Services.GetRequiredService<HomePageViewModel>()
        });
    }

    public object? CurrentPage => _navigationService.CurrentItem?.Factory?.Invoke();
}
```

### ContentDialog Service

```csharp
public class YourViewModel : ViewModelBase
{
    private readonly IContentDialogService _dialogService;

    public YourViewModel(IContentDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public async Task ShowDialogAsync()
    {
        var result = await _dialogService.ShowAsync(
            title: "Confirm Action",
            content: "Are you sure you want to proceed?",
            primaryButtonText: "Yes",
            closeButtonText: "No"
        );

        if (result == ContentDialogResult.Primary)
        {
            // User clicked Yes
        }
    }
}
```

### InfoBar Service

```csharp
_infoBarService.Show(
    message: "Operation completed successfully",
    severity: InfoBarSeverity.Success,
    isClosable: true
);
```

### Overlay Service

```csharp
_overlayService.Show(
    content: new LoadingView(),
    isModal: true
);

// Later...
_overlayService.Hide();
```

## Project Structure

```
Cobalt.Avalonia.Desktop/
├── Controls/
│   ├── Navigation/          # Navigation controls
│   ├── Docking/            # Docking system
│   ├── Ribbon/             # Ribbon controls
│   ├── Editors/            # Editor controls
│   ├── CalendarSchedule/   # Calendar components
│   ├── ContentDialog.cs    # Dialog control
│   ├── OverlayControl.cs   # Overlay control
│   ├── InfoBarControl.cs   # InfoBar control
│   └── SettingsCard*.cs    # Settings controls
├── Services/               # Service implementations
│   ├── NavigationService.cs
│   ├── ContentDialogService.cs
│   ├── InfoBarService.cs
│   └── OverlayService.cs
└── Themes/                 # AXAML templates & styles
    ├── Fluent.axaml       # Main theme include
    ├── Colors.axaml       # Color definitions
    └── Brushes.axaml      # Brush resources
```

## Key Patterns

- **TemplatedControls**: All custom controls use `TemplatedControl` pattern, not `UserControl`
- **ViewLocator**: Automatic view resolution by replacing "ViewModel" → "View" in type names
- **Service Host Pattern**: Services require host controls registered at application startup
- **Two-Way Bindings**: Use `{Binding ..., RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}` in templates (not `{TemplateBinding}`)
- **Theme Resources**: Use `{DynamicResource}` for theme-reactive styling

## Theme System

Access theme colors and brushes using dynamic resources:

```xml
<Border Background="{DynamicResource CobaltSurfacePrimaryBrush}"
        BorderBrush="{DynamicResource CobaltBorderPrimaryBrush}">
```

Available resource prefixes:
- `CobaltBackground*` - Background colors
- `CobaltSurface*` - Surface/card backgrounds
- `CobaltBorder*` - Border colors
- `CobaltForeground*` - Text colors
- `CobaltAccent*` - Accent colors
- `CobaltSuccess*`, `CobaltWarning*`, `CobaltError*` - Semantic colors

## Development

### Build

```bash
dotnet build
```

### Run Tester Application

```bash
dotnet run --project CobaltAvaloniaDesktopTester
```

The tester application (`CobaltAvaloniaDesktopTester`) provides live examples and testing playground for all controls.

## License

[Add your license information here]

## Contributing

[Add contributing guidelines here]


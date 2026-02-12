using Cobalt.Avalonia.Desktop.Controls.Navigation;

namespace Cobalt.Avalonia.Desktop.Services;

/// <summary>
/// Provides context information during navigation lifecycle events.
/// </summary>
public class NavigationContext
{
    /// <summary>
    /// The page being navigated to (may be null if navigation target hasn't been created yet).
    /// </summary>
    public object? TargetPage { get; init; }

    /// <summary>
    /// The navigation item being navigated to (may be null for programmatic navigation).
    /// </summary>
    public NavigationItemControl? TargetItem { get; init; }
}

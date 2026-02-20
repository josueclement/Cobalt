using Cobalt.Localization.Services;

namespace Cobalt.Localization.Avalonia;

public static class LocalizationManager
{
    private static ILocalizationService? _service;

    public static ILocalizationService Service =>
        _service ?? throw new InvalidOperationException(
            "LocalizationManager has not been initialized. Call LocalizationManager.Initialize() at app startup.");

    public static void Initialize(ILocalizationService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }
}

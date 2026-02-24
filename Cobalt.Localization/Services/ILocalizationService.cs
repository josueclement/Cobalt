using System;

namespace Cobalt.Localization.Services
{
    public interface ILocalizationService
    {
        string CurrentLanguage { get; set; }

        event EventHandler<string> CurrentLanguageChanged;

        string? GetTranslation(string key);

        string GetTranslation(string key, string defaultText);
    }
}

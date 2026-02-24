using System;
using Cobalt.Localization.Repositories;

namespace Cobalt.Localization.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ITranslationRepository _repository;
        private string _currentLanguage;

        public LocalizationService(ITranslationRepository repository, string defaultLanguage)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _currentLanguage = defaultLanguage ?? throw new ArgumentNullException(nameof(defaultLanguage));
        }

        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (string.Equals(_currentLanguage, value, StringComparison.Ordinal))
                    return;

                _currentLanguage = value;
                CurrentLanguageChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<string>? CurrentLanguageChanged;

        public string? GetTranslation(string key)
        {
            return _repository.GetByKey(_currentLanguage, key)?.Text;
        }

        public string GetTranslation(string key, string defaultText)
        {
            return GetTranslation(key) ?? defaultText;
        }
    }
}

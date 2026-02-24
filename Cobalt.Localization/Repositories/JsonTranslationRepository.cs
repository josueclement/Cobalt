using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cobalt.Localization.Models;
using Newtonsoft.Json;

namespace Cobalt.Localization.Repositories
{
    public class JsonTranslationRepository : ITranslationRepository
    {
        private readonly string _filePath;
        private readonly List<Translation> _translations;

        public JsonTranslationRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _translations = Load();
        }

        public IReadOnlyList<Translation> GetAll()
        {
            return _translations.AsReadOnly();
        }

        public IReadOnlyList<Translation> GetByLanguage(string languageCode)
        {
            return _translations
                .Where(t => string.Equals(t.LanguageCode, languageCode, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .AsReadOnly();
        }

        public Translation? GetByKey(string languageCode, string key)
        {
            return _translations.FirstOrDefault(t =>
                string.Equals(t.LanguageCode, languageCode, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(t.Key, key, StringComparison.Ordinal));
        }

        public void Add(Translation translation)
        {
            if (translation == null) throw new ArgumentNullException(nameof(translation));

            var existing = GetByKey(translation.LanguageCode, translation.Key);
            if (existing != null)
                throw new InvalidOperationException($"Translation already exists for language '{translation.LanguageCode}' and key '{translation.Key}'.");

            _translations.Add(translation);
        }

        public void Update(Translation translation)
        {
            if (translation == null) throw new ArgumentNullException(nameof(translation));

            var existing = GetByKey(translation.LanguageCode, translation.Key);
            if (existing == null)
                throw new InvalidOperationException($"Translation not found for language '{translation.LanguageCode}' and key '{translation.Key}'.");

            existing.Text = translation.Text;
        }

        public void Delete(string languageCode, string key)
        {
            var existing = GetByKey(languageCode, key);
            if (existing == null)
                throw new InvalidOperationException($"Translation not found for language '{languageCode}' and key '{key}'.");

            _translations.Remove(existing);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(_translations, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        private List<Translation> Load()
        {
            if (!File.Exists(_filePath))
                return new List<Translation>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Translation>>(json) ?? new List<Translation>();
        }
    }
}

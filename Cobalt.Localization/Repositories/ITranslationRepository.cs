using System.Collections.Generic;
using Cobalt.Localization.Models;

namespace Cobalt.Localization.Repositories
{
    public interface ITranslationRepository
    {
        IReadOnlyList<Translation> GetAll();
        IReadOnlyList<Translation> GetByLanguage(string languageCode);
        Translation? GetByKey(string languageCode, string key);
        void Add(Translation translation);
        void Update(Translation translation);
        void Delete(string languageCode, string key);
        void Save();
    }
}

using System.Collections.Generic;
using Cobalt.Iam.Models;

namespace Cobalt.Iam.Repositories
{
    public interface ISessionRepository
    {
        IReadOnlyList<Session> GetAll();
        Session? GetByToken(string token);
        IReadOnlyList<Session> GetByUserId(string userId);
        void Add(Session session);
        void Delete(string token);
        void DeleteExpired();
        void Save();
    }
}

using System.Collections.Generic;
using Cobalt.Iam.Models;

namespace Cobalt.Iam.Repositories
{
    public interface IUserRepository
    {
        IReadOnlyList<User> GetAll();
        User? GetById(string id);
        User? GetByUsername(string username);
        void Add(User user);
        void Update(User user);
        void Delete(string id);
        void Save();
    }
}

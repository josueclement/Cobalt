using System.Collections.Generic;
using Cobalt.Iam.Models;

namespace Cobalt.Iam.Repositories
{
    public interface IRoleRepository
    {
        IReadOnlyList<Role> GetAll();
        Role? GetById(string id);
        void Add(Role role);
        void Update(Role role);
        void Delete(string id);
        void Save();
    }
}

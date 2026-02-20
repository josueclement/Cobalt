using System.Collections.Generic;
using Cobalt.Iam.Models;

namespace Cobalt.Iam.Repositories
{
    public interface IGroupRepository
    {
        IReadOnlyList<Group> GetAll();
        Group? GetById(string id);
        void Add(Group group);
        void Update(Group group);
        void Delete(string id);
        void Save();
    }
}

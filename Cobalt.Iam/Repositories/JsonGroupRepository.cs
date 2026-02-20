using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cobalt.Iam.Models;
using Newtonsoft.Json;

namespace Cobalt.Iam.Repositories
{
    public class JsonGroupRepository : IGroupRepository
    {
        private readonly string _filePath;
        private readonly List<Group> _groups;

        public JsonGroupRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _groups = Load();
        }

        public IReadOnlyList<Group> GetAll()
        {
            return _groups.AsReadOnly();
        }

        public Group? GetById(string id)
        {
            return _groups.FirstOrDefault(g => string.Equals(g.Id, id, StringComparison.Ordinal));
        }

        public void Add(Group group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            var existing = GetById(group.Id);
            if (existing != null)
                throw new InvalidOperationException($"Group with id '{group.Id}' already exists.");

            _groups.Add(group);
        }

        public void Update(Group group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            var existing = GetById(group.Id);
            if (existing == null)
                throw new InvalidOperationException($"Group with id '{group.Id}' not found.");

            existing.Name = group.Name;
            existing.RoleIds = group.RoleIds;
        }

        public void Delete(string id)
        {
            var existing = GetById(id);
            if (existing == null)
                throw new InvalidOperationException($"Group with id '{id}' not found.");

            _groups.Remove(existing);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(_groups, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        private List<Group> Load()
        {
            if (!File.Exists(_filePath))
                return new List<Group>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Group>>(json) ?? new List<Group>();
        }
    }
}

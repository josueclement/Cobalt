using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cobalt.Iam.Models;
using Newtonsoft.Json;

namespace Cobalt.Iam.Repositories
{
    public class JsonRoleRepository : IRoleRepository
    {
        private readonly string _filePath;
        private readonly List<Role> _roles;

        public JsonRoleRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _roles = Load();
        }

        public IReadOnlyList<Role> GetAll()
        {
            return _roles.AsReadOnly();
        }

        public Role? GetById(string id)
        {
            return _roles.FirstOrDefault(r => string.Equals(r.Id, id, StringComparison.Ordinal));
        }

        public void Add(Role role)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            var existing = GetById(role.Id);
            if (existing != null)
                throw new InvalidOperationException($"Role with id '{role.Id}' already exists.");

            _roles.Add(role);
        }

        public void Update(Role role)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            var existing = GetById(role.Id);
            if (existing == null)
                throw new InvalidOperationException($"Role with id '{role.Id}' not found.");

            existing.Name = role.Name;
        }

        public void Delete(string id)
        {
            var existing = GetById(id);
            if (existing == null)
                throw new InvalidOperationException($"Role with id '{id}' not found.");

            _roles.Remove(existing);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(_roles, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        private List<Role> Load()
        {
            if (!File.Exists(_filePath))
                return new List<Role>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Role>>(json) ?? new List<Role>();
        }
    }
}

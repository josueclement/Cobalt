using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cobalt.Iam.Models;
using Newtonsoft.Json;

namespace Cobalt.Iam.Repositories
{
    public class JsonUserRepository : IUserRepository
    {
        private readonly string _filePath;
        private readonly List<User> _users;

        public JsonUserRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _users = Load();
        }

        public IReadOnlyList<User> GetAll()
        {
            return _users.AsReadOnly();
        }

        public User? GetById(string id)
        {
            return _users.FirstOrDefault(u => string.Equals(u.Id, id, StringComparison.Ordinal));
        }

        public User? GetByUsername(string username)
        {
            return _users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
        }

        public void Add(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var existing = GetByUsername(user.Username);
            if (existing != null)
                throw new InvalidOperationException($"User with username '{user.Username}' already exists.");

            _users.Add(user);
        }

        public void Update(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var existing = GetById(user.Id);
            if (existing == null)
                throw new InvalidOperationException($"User with id '{user.Id}' not found.");

            existing.Username = user.Username;
            existing.PasswordHash = user.PasswordHash;
            existing.Email = user.Email;
            existing.RoleIds = user.RoleIds;
            existing.GroupIds = user.GroupIds;
        }

        public void Delete(string id)
        {
            var existing = GetById(id);
            if (existing == null)
                throw new InvalidOperationException($"User with id '{id}' not found.");

            _users.Remove(existing);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(_users, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        private List<User> Load()
        {
            if (!File.Exists(_filePath))
                return new List<User>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }
    }
}

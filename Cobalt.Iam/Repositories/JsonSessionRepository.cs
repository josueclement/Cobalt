using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cobalt.Iam.Models;
using Newtonsoft.Json;

namespace Cobalt.Iam.Repositories
{
    public class JsonSessionRepository : ISessionRepository
    {
        private readonly string _filePath;
        private readonly List<Session> _sessions;

        public JsonSessionRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _sessions = Load();
        }

        public IReadOnlyList<Session> GetAll()
        {
            return _sessions.AsReadOnly();
        }

        public Session? GetByToken(string token)
        {
            return _sessions.FirstOrDefault(s => string.Equals(s.Id, token, StringComparison.Ordinal));
        }

        public IReadOnlyList<Session> GetByUserId(string userId)
        {
            return _sessions
                .Where(s => string.Equals(s.UserId, userId, StringComparison.Ordinal))
                .ToList()
                .AsReadOnly();
        }

        public void Add(Session session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            var existing = GetByToken(session.Id);
            if (existing != null)
                throw new InvalidOperationException($"Session with id '{session.Id}' already exists.");

            _sessions.Add(session);
        }

        public void Delete(string token)
        {
            var existing = GetByToken(token);
            if (existing == null)
                throw new InvalidOperationException($"Session with token '{token}' not found.");

            _sessions.Remove(existing);
        }

        public void DeleteExpired()
        {
            _sessions.RemoveAll(s => s.ExpiresAt < DateTime.UtcNow);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(_sessions, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        private List<Session> Load()
        {
            if (!File.Exists(_filePath))
                return new List<Session>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Session>>(json) ?? new List<Session>();
        }
    }
}

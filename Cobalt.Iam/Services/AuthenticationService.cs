using System;
using System.Security.Cryptography;
using System.Text;
using Cobalt.Iam.Models;
using Cobalt.Iam.Repositories;

namespace Cobalt.Iam.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly TimeSpan _sessionDuration;

        public AuthenticationService(
            IUserRepository userRepository,
            ISessionRepository sessionRepository,
            TimeSpan sessionDuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
            _sessionDuration = sessionDuration;
        }

        public Session? Login(string username, string password)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var user = _userRepository.GetByUsername(username);
            if (user == null)
                return null;

            var hash = HashPassword(password);
            if (!string.Equals(user.PasswordHash, hash, StringComparison.Ordinal))
                return null;

            var session = new Session
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.Add(_sessionDuration)
            };

            _sessionRepository.Add(session);
            _sessionRepository.Save();

            return session;
        }

        public void Logout(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            _sessionRepository.Delete(token);
            _sessionRepository.Save();
        }

        public User? ValidateSession(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            var session = _sessionRepository.GetByToken(token);
            if (session == null)
                return null;

            if (session.ExpiresAt < DateTime.UtcNow)
            {
                _sessionRepository.Delete(token);
                _sessionRepository.Save();
                return null;
            }

            return _userRepository.GetById(session.UserId);
        }

        public string HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public User Register(string username, string password, string email)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (email == null) throw new ArgumentNullException(nameof(email));

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                PasswordHash = HashPassword(password),
                Email = email
            };

            _userRepository.Add(user);
            _userRepository.Save();

            return user;
        }
    }
}

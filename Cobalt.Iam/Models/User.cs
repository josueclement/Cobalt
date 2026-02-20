using System.Collections.Generic;

namespace Cobalt.Iam.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> RoleIds { get; set; } = new List<string>();
        public List<string> GroupIds { get; set; } = new List<string>();
    }
}

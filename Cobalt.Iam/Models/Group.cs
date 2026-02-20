using System.Collections.Generic;

namespace Cobalt.Iam.Models
{
    public class Group
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<string> RoleIds { get; set; } = new List<string>();
    }
}

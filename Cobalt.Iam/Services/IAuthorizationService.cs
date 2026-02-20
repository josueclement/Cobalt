using System.Collections.Generic;
using Cobalt.Iam.Models;

namespace Cobalt.Iam.Services
{
    public interface IAuthorizationService
    {
        bool HasRole(string userId, string roleName);
        IReadOnlyList<Role> GetEffectiveRoles(string userId);
        void AssignRoleToUser(string userId, string roleId);
        void RemoveRoleFromUser(string userId, string roleId);
        void AssignUserToGroup(string userId, string groupId);
        void RemoveUserFromGroup(string userId, string groupId);
    }
}

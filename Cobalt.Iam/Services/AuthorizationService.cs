using System;
using System.Collections.Generic;
using System.Linq;
using Cobalt.Iam.Models;
using Cobalt.Iam.Repositories;

namespace Cobalt.Iam.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthorizationService(
            IUserRepository userRepository,
            IGroupRepository groupRepository,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public bool HasRole(string userId, string roleName)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));

            var roles = GetEffectiveRoles(userId);
            return roles.Any(r => string.Equals(r.Name, roleName, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<Role> GetEffectiveRoles(string userId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            var user = _userRepository.GetById(userId);
            if (user == null)
                throw new InvalidOperationException($"User with id '{userId}' not found.");

            var roleIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var roleId in user.RoleIds)
            {
                roleIds.Add(roleId);
            }

            foreach (var groupId in user.GroupIds)
            {
                var group = _groupRepository.GetById(groupId);
                if (group != null)
                {
                    foreach (var roleId in group.RoleIds)
                    {
                        roleIds.Add(roleId);
                    }
                }
            }

            var roles = new List<Role>();
            foreach (var roleId in roleIds)
            {
                var role = _roleRepository.GetById(roleId);
                if (role != null)
                {
                    roles.Add(role);
                }
            }

            return roles.AsReadOnly();
        }

        public void AssignRoleToUser(string userId, string roleId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (roleId == null) throw new ArgumentNullException(nameof(roleId));

            var user = _userRepository.GetById(userId);
            if (user == null)
                throw new InvalidOperationException($"User with id '{userId}' not found.");

            var role = _roleRepository.GetById(roleId);
            if (role == null)
                throw new InvalidOperationException($"Role with id '{roleId}' not found.");

            if (!user.RoleIds.Contains(roleId))
            {
                user.RoleIds.Add(roleId);
                _userRepository.Save();
            }
        }

        public void RemoveRoleFromUser(string userId, string roleId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (roleId == null) throw new ArgumentNullException(nameof(roleId));

            var user = _userRepository.GetById(userId);
            if (user == null)
                throw new InvalidOperationException($"User with id '{userId}' not found.");

            if (user.RoleIds.Remove(roleId))
            {
                _userRepository.Save();
            }
        }

        public void AssignUserToGroup(string userId, string groupId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (groupId == null) throw new ArgumentNullException(nameof(groupId));

            var user = _userRepository.GetById(userId);
            if (user == null)
                throw new InvalidOperationException($"User with id '{userId}' not found.");

            var group = _groupRepository.GetById(groupId);
            if (group == null)
                throw new InvalidOperationException($"Group with id '{groupId}' not found.");

            if (!user.GroupIds.Contains(groupId))
            {
                user.GroupIds.Add(groupId);
                _userRepository.Save();
            }
        }

        public void RemoveUserFromGroup(string userId, string groupId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (groupId == null) throw new ArgumentNullException(nameof(groupId));

            var user = _userRepository.GetById(userId);
            if (user == null)
                throw new InvalidOperationException($"User with id '{userId}' not found.");

            if (user.GroupIds.Remove(groupId))
            {
                _userRepository.Save();
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using TMS.Abstractions.Enums;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Services;
using TMS.Infrastructure.Abstractions.Repositories;

namespace TMS.Application.Security
{
    public class AccessService : IAccessService
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AccessService> _logger;

        private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(10);

        public AccessService(
            IMembershipRepository membershipRepository,
            IRoleRepository roleRepository,
            IRolePermissionRepository rolePermissionRepository,
            ICacheService cacheService,
            ILogger<AccessService> logger)
        {
            _membershipRepository = membershipRepository ?? throw new ArgumentNullException(nameof(membershipRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _rolePermissionRepository = rolePermissionRepository ?? throw new ArgumentNullException(nameof(rolePermissionRepository));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> HasPermissionAsync(
            Guid userId,
            Guid resourceId,
            ResourceType resourceType,
            string permissionName,
            CancellationToken cancellationToken = default)
        {
            var cachedPermissions = await _cacheService.GetAsync<List<string>>(CacheKeys.Permissions(userId, resourceType, resourceId));
            if (cachedPermissions != null)
            {
                _logger.LogDebug("Permissions for ({UserId},{ResourceId},{ResourceType}) found in cache.", userId, resourceId, resourceType);
                return cachedPermissions.Contains(permissionName, StringComparer.OrdinalIgnoreCase);
            }

            var membership = await _membershipRepository.GetUserMembershipAsync(userId, resourceId, (int)resourceType, cancellationToken);
            if (membership == null)
            {
                _logger.LogDebug("No membership for ({UserId},{ResourceId},{ResourceType})", userId, resourceId, resourceType);
                return false;
            }

            var role = await _roleRepository.GetByIdAsync(membership.RoleId, cancellationToken);
            if (role == null)
            {
                _logger.LogWarning("Role {RoleId} for membership not found", membership.RoleId);
                return false;
            }

            var permissions = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(role.Id, cancellationToken);
            var permissionNames = permissions
                .Where(p => p.DeleteDate == null)
                .Select(p => p.Name)
                .Distinct()
                .ToList();

            await _cacheService.SetAsync(CacheKeys.Permissions(userId, resourceType, resourceId), permissionNames, CacheExpiry);

            // 6. Проверяем наличие нужного permission
            return permissionNames.Contains(permissionName, StringComparer.OrdinalIgnoreCase);
        }
    }
}

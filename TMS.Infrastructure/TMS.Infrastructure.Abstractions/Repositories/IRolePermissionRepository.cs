using System.Data;
using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    public interface IRolePermissionRepository :
        IAuditableCommandRepository<RolePermission>,
        IAuditableQueryRepository<RolePermission>
    {
        Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);
    }
}

using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    public interface IPermissionRepository :
        IAuditableCommandRepository<Permission>,
        IAuditableQueryRepository<Permission>
    {
    }
}

using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    /// <summary>
    /// Role-specific repository with additional role-related queries, extending <see cref="IAuditableCommandRepository{Role}"/> and <see cref="IAuditableQueryRepository{Role}"/>.
    /// </summary>
    public interface IRoleRepository :
        IAuditableCommandRepository<Role>,
        IAuditableQueryRepository<Role>
    {
        Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
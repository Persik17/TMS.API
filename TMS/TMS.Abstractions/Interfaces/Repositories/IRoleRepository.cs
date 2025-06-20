using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// Role-specific repository with additional role-related queries, extending <see cref="IAuditableCommandRepository{TEntity}"/> and <see cref="IAuditableQueryRepository{TEntity}"/>.
    /// </summary>
    public interface IRoleRepository<TEntity> :
        IAuditableCommandRepository<TEntity>,
        IAuditableQueryRepository<TEntity>
        where TEntity : class, IAuditableEntity
    {
    }
}
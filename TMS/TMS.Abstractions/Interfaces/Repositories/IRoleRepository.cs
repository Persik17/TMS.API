using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// Role-specific repository with additional role-related queries.
    /// </summary>
    public interface IRoleRepository<TEntity> :
        IAuditableCommandRepository<TEntity>,
        IAuditableQueryRepository<TEntity>
        where TEntity : class, IEntity, IAuditableEntity
    {
    }
}
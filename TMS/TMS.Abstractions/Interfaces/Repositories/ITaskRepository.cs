using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// Task-specific repository with additional task-related queries.
    /// </summary>
    public interface ITaskRepository<TEntity> :
        IAuditableCommandRepository<TEntity>,
        IAuditableQueryRepository<TEntity>
        where TEntity : class, IEntity, IAuditableEntity
    {
    }
}
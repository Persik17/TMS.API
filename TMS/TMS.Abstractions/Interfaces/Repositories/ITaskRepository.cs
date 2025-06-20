using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// Task-specific repository with additional task-related queries, extending <see cref="IAuditableCommandRepository{TEntity}"/> and <see cref="IAuditableQueryRepository{TEntity}"/>.
    /// </summary>
    public interface ITaskRepository<TEntity> :
        IAuditableCommandRepository<TEntity>,
        IAuditableQueryRepository<TEntity>
        where TEntity : class, IAuditableEntity
    {
    }
}
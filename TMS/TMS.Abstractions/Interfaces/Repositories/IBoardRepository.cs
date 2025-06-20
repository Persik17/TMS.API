using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// Board-specific repository with additional board-related queries, extending <see cref="IAuditableCommandRepository{TEntity}"/> and <see cref="IAuditableQueryRepository{TEntity}"/>.
    /// </summary>
    public interface IBoardRepository<TEntity> :
        IAuditableCommandRepository<TEntity>,
        IAuditableQueryRepository<TEntity>
        where TEntity : class, IAuditableEntity
    {
    }
}
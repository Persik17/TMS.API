using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// Board-specific repository with additional board-related queries.
    /// </summary>
    public interface IBoardRepository<TEntity> :
        IAuditableCommandRepository<TEntity>,
        IAuditableQueryRepository<TEntity>
        where TEntity : class, IEntity, IAuditableEntity
    {
    }
}
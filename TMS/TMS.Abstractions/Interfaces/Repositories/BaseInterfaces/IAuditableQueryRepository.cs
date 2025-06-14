using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Repositories.BaseInterfaces
{
    /// <summary>
    /// Extends IQueryRepository for auditable entities.
    /// </summary>
    /// <typeparam name="TEntity">The auditable entity type.</typeparam>
    public interface IAuditableQueryRepository<TEntity> : IQueryRepository<TEntity>
        where TEntity : class, IEntity, IAuditableEntity
    {
    }
}

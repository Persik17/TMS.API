using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Repositories.BaseInterfaces
{
    /// <summary>
    /// Extends ICommandRepository for auditable entities.
    /// </summary>
    /// <typeparam name="TEntity">The auditable entity type.</typeparam>
    public interface IAuditableCommandRepository<TEntity> : ICommandRepository<TEntity>
        where TEntity : class, IEntity, IAuditableEntity
    {
    }
}

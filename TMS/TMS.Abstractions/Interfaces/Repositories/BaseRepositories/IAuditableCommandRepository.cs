using TMS.Abstractions.Models.Contracts;

namespace TMS.Abstractions.Interfaces.Repositories.BaseRepositories
{
    /// <summary>
    /// Extends <see cref="ICommandRepository{TEntity}"/> for auditable entities.
    /// </summary>
    /// <typeparam name="TEntity">The auditable entity type.  Must implement <see cref="IAuditableEntity"/>.</typeparam>
    public interface IAuditableCommandRepository<TEntity> : ICommandRepository<TEntity>
        where TEntity : class, IAuditableEntity
    {
    }
}

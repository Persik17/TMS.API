using TMS.Abstractions.Models.Contracts;

namespace TMS.Abstractions.Interfaces.Repositories.BaseRepositories
{
    /// <summary>
    /// Extends <see cref="IQueryRepository{TEntity}"/> for auditable entities.
    /// </summary>
    /// <typeparam name="TEntity">The auditable entity type. Must implement <see cref="IAuditableEntity"/>.</typeparam>
    public interface IAuditableQueryRepository<TEntity> : IQueryRepository<TEntity>
        where TEntity : class, IAuditableEntity
    {
    }
}

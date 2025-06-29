using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.Abstractions.Repositories.BaseRepositories
{
    /// <summary>
    /// Defines basic read operations for an entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type. Must implement <see cref="IEntity"/>.</typeparam>
    public interface IQueryRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Gets an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the entity, or <c>null</c> if not found.</returns>
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous get all operation. The task result contains a collection of entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}

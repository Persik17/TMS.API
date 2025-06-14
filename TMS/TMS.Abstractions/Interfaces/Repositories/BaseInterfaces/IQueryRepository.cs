using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Repositories.BaseInterfaces
{
    /// <summary>
    /// Defines basic read operations for an entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IQueryRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The entity or null if not found.</returns>
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}

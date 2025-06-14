using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Repositories.BaseInterfaces
{
    /// Defines basic write operations for an entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface ICommandRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="entityId">The identifier of the entity to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default);
    }
}

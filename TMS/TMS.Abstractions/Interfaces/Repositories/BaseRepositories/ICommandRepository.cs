using TMS.Abstractions.Models.Contracts;

namespace TMS.Abstractions.Interfaces.Repositories.BaseRepositories
{
    /// <summary>
    /// Defines basic write operations for an entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type. Must implement <see cref="IEntity"/>.</typeparam>
    public interface ICommandRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous insert operation.</returns>
        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="entityId">The identifier of the entity to delete.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default);
    }
}

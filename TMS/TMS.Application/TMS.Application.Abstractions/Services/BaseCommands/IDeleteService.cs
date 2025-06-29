namespace TMS.Application.Abstractions.Services.BaseCommands
{
    /// <summary>
    /// Defines a contract for deleting entities.
    /// </summary>
    public interface IDeleteService
    {
        /// <summary>
        /// Deletes an entity by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        /// <exception cref="NotFoundException">Thrown when an entity with the specified <paramref name="id"/> is not found.</exception>
        Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    }
}
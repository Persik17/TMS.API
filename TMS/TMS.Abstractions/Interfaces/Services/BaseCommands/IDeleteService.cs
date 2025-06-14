namespace TMS.Abstractions.Interfaces.Services.BaseCommands
{
    /// <summary>
    /// Defines a contract for deleting entities.
    /// </summary>
    public interface IDeleteService
    {
        /// <summary>
        /// Deletes an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
namespace TMS.Application.Abstractions.Services.BaseCommands
{
    /// <summary>
    /// Defines a contract for reading entities.
    /// </summary>
    /// <typeparam name="TReadDto">The DTO or ViewModel used for reading the entity.</typeparam>
    public interface IReadService<TReadDto>
        where TReadDto : class
    {
        /// <summary>
        /// Retrieves an entity by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous read operation. The task result contains the entity as a read model, or <c>null</c> if not found.</returns>
        Task<TReadDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    }
}
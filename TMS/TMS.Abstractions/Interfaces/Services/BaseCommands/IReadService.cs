namespace TMS.Abstractions.Interfaces.Services.BaseCommands
{
    /// <summary>
    /// Defines a contract for reading entities.
    /// </summary>
    /// <typeparam name="TReadDto">DTO or ViewModel used for reading the entity.</typeparam>
    public interface IReadService<TReadDto>
        where TReadDto : class
    {
        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The entity as a read model, or null if not found.</returns>
        Task<TReadDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
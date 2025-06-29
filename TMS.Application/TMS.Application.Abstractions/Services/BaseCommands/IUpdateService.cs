namespace TMS.Application.Abstractions.Services.BaseCommands
{
    /// <summary>
    /// Defines a contract for updating entities.
    /// </summary>
    /// <typeparam name="TReadDto">The DTO or ViewModel used for updating the entity.</typeparam>
    public interface IUpdateService<TReadDto>
        where TReadDto : class
    {
        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing updated entity data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous update operation. The task result contains the updated entity as a read model.</returns>
        Task<TReadDto> UpdateAsync(TReadDto dto, Guid userId, CancellationToken cancellationToken = default);
    }
}
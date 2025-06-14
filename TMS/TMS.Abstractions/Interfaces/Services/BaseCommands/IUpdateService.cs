namespace TMS.Abstractions.Interfaces.Services.BaseCommands
{
    /// <summary>
    /// Defines a contract for updating entities.
    /// </summary>
    /// <typeparam name="TReadDto">DTO or ViewModel used for updating the entity.</typeparam>
    public interface IUpdateService<TReadDto>
        where TReadDto : class
    {
        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="dto">The DTO containing updated entity data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated entity as a read model.</returns>
        Task<TReadDto> UpdateAsync(TReadDto dto, CancellationToken cancellationToken = default);
    }
}
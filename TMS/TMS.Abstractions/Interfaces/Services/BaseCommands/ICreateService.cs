namespace TMS.Abstractions.Interfaces.Services.BaseCommands
{
    /// <summary>
    /// Defines a contract for creating entities.
    /// </summary>
    /// <typeparam name="TCreateDto">DTO used for creating a new entity.</typeparam>
    /// <typeparam name="TReadDto">DTO or ViewModel used for reading the created entity.</typeparam>
    public interface ICreateService<TCreateDto, TReadDto>
        where TCreateDto : class
        where TReadDto : class
    {
        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="createDto">The DTO containing data for the new entity.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created entity as a read model.</returns>
        Task<TReadDto> CreateAsync(TCreateDto createDto, CancellationToken cancellationToken = default);
    }
}





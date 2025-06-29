namespace TMS.Application.Abstractions.Services.BaseCommands
{
    /// <summary>
    /// Defines a contract for creating entities.
    /// </summary>
    /// <typeparam name="TCreateDto">The DTO used for creating a new entity.</typeparam>
    /// <typeparam name="TReadDto">The DTO or ViewModel used for reading the created entity.</typeparam>
    public interface ICreateService<TCreateDto, TReadDto>
        where TCreateDto : class
        where TReadDto : class
    {
        /// <summary>
        /// Creates a new entity asynchronously.
        /// </summary>
        /// <param name="createDto">The DTO containing data for the new entity.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous create operation. The task result contains the created entity as a read model.</returns>
        Task<TReadDto> CreateAsync(TCreateDto createDto, Guid userId, CancellationToken cancellationToken = default);
    }
}





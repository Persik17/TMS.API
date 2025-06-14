using TMS.Abstractions.Interfaces.Services.BaseCommands;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing Board entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    /// <typeparam name="TReadModel">DTO or ViewModel used for reading and updating board data.</typeparam>
    /// <typeparam name="TCreateModel">DTO used for creating a new board.</typeparam>
    public interface IBoardService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>,
        IDeleteService
        where TReadModel : class
        where TCreateModel : class
    {
        // <inheritdoc cref="ICreateService{TCreateModel, TReadModel}.CreateAsync"/>
        // Creates a new board entity.
        // <param name="createDto">The DTO containing data for the new board.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The created board as a read model.</returns>

        // <inheritdoc cref="IReadService{TReadModel}.GetByIdAsync"/>
        // Retrieves a board entity by its unique identifier.
        // <param name="id">The unique identifier of the board.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The board as a read model, or null if not found.</returns>

        // <inheritdoc cref="IUpdateService{TReadModel}.UpdateAsync"/>
        // Updates an existing board entity.
        // <param name="dto">The DTO containing updated v data.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The updated board as a read model.</returns>

        // <inheritdoc cref="IDeleteService.DeleteAsync"/>
        // Deletes a board entity by its unique identifier.
        // <param name="id">The unique identifier of the board to delete.</param>
        // <param name="cancellationToken">Cancellation token.</param>    }
    }
}
using TMS.Abstractions.Interfaces.Services.BaseCommands;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing Column entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    /// <typeparam name="TReadModel">DTO or ViewModel used for reading and updating column data.</typeparam>
    /// <typeparam name="TCreateModel">DTO used for creating a new column.</typeparam>
    public interface IColumnService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>,
        IDeleteService
        where TReadModel : class
        where TCreateModel : class
    {
        // <inheritdoc cref="ICreateService{TCreateModel, TReadModel}.CreateAsync"/>
        // Creates a new column entity.
        // <param name="createDto">The DTO containing data for the new column.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The created column as a read model.</returns>

        // <inheritdoc cref="IReadService{TReadModel}.GetByIdAsync"/>
        // Retrieves a column entity by its unique identifier.
        // <param name="id">The unique identifier of the column.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The column as a read model, or null if not found.</returns>

        // <inheritdoc cref="IUpdateService{TReadModel}.UpdateAsync"/>
        // Updates an existing column entity.
        // <param name="dto">The DTO containing updated column data.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The updated column as a read model.</returns>

        // <inheritdoc cref="IDeleteService.DeleteAsync"/>
        // Deletes a column entity by its unique identifier.
        // <param name="id">The unique identifier of the column to delete.</param>
        // <param name="cancellationToken">Cancellation token.</param>
    }
}
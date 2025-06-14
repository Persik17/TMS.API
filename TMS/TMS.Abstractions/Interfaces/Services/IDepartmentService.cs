using TMS.Abstractions.Interfaces.Services.BaseCommands;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing Department entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    /// <typeparam name="TReadModel">DTO or ViewModel used for reading and updating department data.</typeparam>
    /// <typeparam name="TCreateModel">DTO used for creating a new department.</typeparam>
    public interface IDepartmentService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>,
        IDeleteService
        where TReadModel : class
        where TCreateModel : class
    {
        // <inheritdoc cref="ICreateService{TCreateModel, TReadModel}.CreateAsync"/>
        // Creates a new department entity.
        // <param name="createDto">The DTO containing data for the new department.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The created department as a read model.</returns>

        // <inheritdoc cref="IReadService{TReadModel}.GetByIdAsync"/>
        // Retrieves a department entity by its unique identifier.
        // <param name="id">The unique identifier of the department.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The department as a read model, or null if not found.</returns>

        // <inheritdoc cref="IUpdateService{TReadModel}.UpdateAsync"/>
        // Updates an existing department entity.
        // <param name="dto">The DTO containing updated department data.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The updated department as a read model.</returns>

        // <inheritdoc cref="IDeleteService.DeleteAsync"/>
        // Deletes a department entity by its unique identifier.
        // <param name="id">The unique identifier of the department to delete.</param>
        // <param name="cancellationToken">Cancellation token.</param>
    }
}
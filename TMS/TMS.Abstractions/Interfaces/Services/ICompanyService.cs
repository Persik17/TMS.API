using TMS.Abstractions.Interfaces.Services.BaseCommands;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing Company entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    /// <typeparam name="TReadModel">DTO or ViewModel used for reading and updating company data.</typeparam>
    /// <typeparam name="TCreateModel">DTO used for creating a new company.</typeparam>
    public interface ICompanyService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>,
        IDeleteService
        where TReadModel : class
        where TCreateModel : class
    {
        // <inheritdoc cref="ICreateService{TCreateModel, TReadModel}.CreateAsync"/>
        // Creates a new company entity.
        // <param name="createDto">The DTO containing data for the new company.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The created company as a read model.</returns>

        // <inheritdoc cref="IReadService{TReadModel}.GetByIdAsync"/>
        // Retrieves a company entity by its unique identifier.
        // <param name="id">The unique identifier of the company.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The company as a read model, or null if not found.</returns>

        // <inheritdoc cref="IUpdateService{TReadModel}.UpdateAsync"/>
        // Updates an existing company entity.
        // <param name="dto">The DTO containing updated v data.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The updated company as a read model.</returns>

        // <inheritdoc cref="IDeleteService.DeleteAsync"/>
        // Deletes a company entity by its unique identifier.
        // <param name="id">The unique identifier of the company to delete.</param>
        // <param name="cancellationToken">Cancellation token.</param>
    }
}
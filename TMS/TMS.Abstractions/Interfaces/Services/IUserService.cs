using TMS.Abstractions.Interfaces.Services.BaseCommands;
using TMS.Abstractions.Models.DTOs;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing User entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    /// <typeparam name="TReadModel">DTO or ViewModel used for reading and updating user data.</typeparam>
    /// <typeparam name="TCreateModel">DTO used for creating a new user.</typeparam>
    public interface IUserService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>,
        IDeleteService
        where TReadModel : class
        where TCreateModel : class
    {
        // <inheritdoc cref="ICreateService{TCreateModel, TReadModel}.CreateAsync"/>
        // Creates a new user entity.
        // <param name="createDto">The DTO containing data for the new user.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The created user as a read model.</returns>

        // <inheritdoc cref="IReadService{TReadModel}.GetByIdAsync"/>
        // Retrieves a user entity by its unique identifier.
        // <param name="id">The unique identifier of the user.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The user as a read model, or null if not found.</returns>

        // <inheritdoc cref="IUpdateService{TReadModel}.UpdateAsync"/>
        // Updates an existing user entity.
        // <param name="dto">The DTO containing updated user data.</param>
        // <param name="cancellationToken">Cancellation token.</param>
        // <returns>The updated user as a read model.</returns>

        // <inheritdoc cref="IDeleteService.DeleteAsync"/>
        // Deletes a user entity by its unique identifier.
        // <param name="id">The unique identifier of the user to delete.</param>
        // <param name="cancellationToken">Cancellation token.</param>

        /// <summary>
        /// Links a Telegram account to the user.
        /// </summary>
        Task LinkTelegramAccountAsync(Guid userId, TelegramAccountCreateDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unlinks the Telegram account from the user.
        /// </summary>
        Task UnlinkTelegramAccountAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}

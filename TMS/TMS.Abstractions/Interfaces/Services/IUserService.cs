using TMS.Abstractions.Interfaces.Services.BaseCommands;
using TMS.Abstractions.Models.DTOs.User;

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
        /// <summary>
        /// Links a Telegram account to the user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="dto">The DTO containing Telegram account creation data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous link Telegram account operation.</returns>
        Task LinkTelegramAccountAsync(Guid userId, TelegramAccountCreateDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unlinks the Telegram account from the user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous unlink Telegram account operation.</returns>
        Task UnlinkTelegramAccountAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}

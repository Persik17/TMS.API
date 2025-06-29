using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    /// <summary>
    /// User-specific repository with additional user-related queries, extending <see cref="IAuditableCommandRepository{User}"/> and <see cref="IAuditableQueryRepository{User}"/>.
    /// </summary>
    public interface IUserRepository :
        IAuditableCommandRepository<User>,
        IAuditableQueryRepository<User>
    {
        /// <summary>
        /// Finds a user by email asynchronously.
        /// </summary>
        /// <param name="email">The email address of the user to find.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous find operation. The task result contains the user, or <c>null</c> if not found.</returns>
        Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a user by their associated Telegram account id.
        /// </summary>
        /// <param name="telegramAccountId">TelegramAccount id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The user, or null if not found.</returns>
        Task<User> GetByTelegramAccountIdAsync(Guid telegramAccountId, CancellationToken cancellationToken = default);
    }
}
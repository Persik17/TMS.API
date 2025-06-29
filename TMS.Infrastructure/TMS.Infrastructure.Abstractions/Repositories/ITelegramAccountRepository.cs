using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    /// <summary>
    /// Repository for accessing Telegram accounts.
    /// </summary>
    public interface ITelegramAccountRepository :
        IAuditableCommandRepository<TelegramAccount>,
        IAuditableQueryRepository<TelegramAccount>
    {
        /// <summary>
        /// Gets a TelegramAccount by the Telegram user id.
        /// </summary>
        /// <param name="telegramUserId">Telegram user id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The Telegram account, or null if not found.</returns>
        Task<TelegramAccount?> GetByTelegramUserIdAsync(long telegramUserId, CancellationToken cancellationToken = default);
    }
}

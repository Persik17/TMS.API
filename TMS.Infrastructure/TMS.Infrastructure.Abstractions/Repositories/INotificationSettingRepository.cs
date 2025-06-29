using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    /// <summary>
    /// Credential-specific repository with additional credential-related queries, extending <see cref="IAuditableCommandRepository{NotificationSetting}"/> and <see cref="IAuditableQueryRepository{NotificationSetting}"/>.
    /// </summary>
    public interface INotificationSettingRepository :
        ICommandRepository<NotificationSetting>,
        IQueryRepository<NotificationSetting>
    {
    }
}

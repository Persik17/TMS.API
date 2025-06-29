using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.NotificationSetting;

namespace TMS.Application.Abstractions.Services
{
    /// <summary>
    /// Service contract for managing NotificationSetting entities.
    /// Provides create, read, and update operations.
    /// </summary>
    public interface INotificationSettingService :
        ICreateService<NotificationSettingCreateDto, NotificationSettingDto>,
        IReadService<NotificationSettingDto>,
        IUpdateService<NotificationSettingDto>
    {
    }
}
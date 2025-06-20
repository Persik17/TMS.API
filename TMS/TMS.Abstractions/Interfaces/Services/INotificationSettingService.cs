using TMS.Abstractions.Interfaces.Services.BaseCommands;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing NotificationSetting entities.
    /// Provides create, read, and update operations.
    /// </summary>
    /// <typeparam name="TReadModel">DTO used for reading notification settings.</typeparam>
    /// <typeparam name="TCreateModel">DTO used for creating notification settings.</typeparam>
    public interface INotificationSettingService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>
        where TReadModel : class
        where TCreateModel : class
    {
    }
}
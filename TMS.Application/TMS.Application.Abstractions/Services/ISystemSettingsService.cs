using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.User;

namespace TMS.Application.Abstractions.Services
{
    public interface ISystemSettingsService :
        ICreateService<SystemSettingsDto, SystemSettingsDto>,
        IReadService<SystemSettingsDto>,
        IUpdateService<SystemSettingsDto>,
        IDeleteService
    {
        Task<SystemSettingsDto?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}

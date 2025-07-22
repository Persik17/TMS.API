using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.User;

namespace TMS.Application.Abstractions.Services
{
    public interface IUserAdminService : 
        ICreateService<UserCreateDto, UserDto>,
        IReadService<UserDto>,
        IUpdateService<UserDto>,
        IDeleteService
    {
        Task InviteByEmailAsync(UserInviteDto dto, Guid adminId, CancellationToken ct = default);
    }
}

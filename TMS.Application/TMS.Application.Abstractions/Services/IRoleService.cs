using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.Role;

namespace TMS.Application.Abstractions.Services
{
    /// <summary>
    /// Service contract for managing Role entities.
    /// Provides CRUD operations.
    /// </summary>
    public interface IRoleService :
        ICreateService<RoleCreateDto, RoleDto>,
        IReadService<RoleDto>,
        IUpdateService<RoleDto>,
        IDeleteService
    {
    }
}
using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.Permission;

namespace TMS.Application.Abstractions.Services
{
    /// <summary>
    /// Service contract for managing Permission entities.
    /// Provides create, read, update, and delete operations.
    /// </summary>
    public interface IPermissionService :
        ICreateService<PermissionCreateDto, PermissionDto>,
        IReadService<PermissionDto>,
        IUpdateService<PermissionDto>,
        IDeleteService
    {
    }
}
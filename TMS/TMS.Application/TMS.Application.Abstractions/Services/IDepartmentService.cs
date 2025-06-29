using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.Department;

namespace TMS.Application.Abstractions.Services
{
    /// <summary>
    /// Service contract for managing Department entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    public interface IDepartmentService :
        ICreateService<DepartmentCreateDto, DepartmentDto>,
        IReadService<DepartmentDto>,
        IUpdateService<DepartmentDto>,
        IDeleteService
    {
    }
}
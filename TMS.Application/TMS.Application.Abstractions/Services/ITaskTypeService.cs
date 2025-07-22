using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.TaskType;

namespace TMS.Application.Abstractions.Services
{
    /// <summary>
    /// Service contract for managing TaskType entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    public interface ITaskTypeService :
        ICreateService<TaskTypeCreateDto, TaskTypeDto>,
        IReadService<TaskTypeDto>,
        IUpdateService<TaskTypeDto>,
        IDeleteService
    {
        Task<List<TaskTypeDto>> GetTasksByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
    }
}
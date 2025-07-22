using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    /// <summary>
    /// Task-specific repository with additional task-related queries, extending <see cref="IAuditableCommandRepository{TaskType}"/> and <see cref="IAuditableQueryRepository{TaskType}"/>.
    /// </summary>
    public interface ITaskTypeRepository :
        IAuditableCommandRepository<TaskType>,
        IAuditableQueryRepository<TaskType>
    {
        Task<List<TaskType>> GetTaskTypesByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
    }
}
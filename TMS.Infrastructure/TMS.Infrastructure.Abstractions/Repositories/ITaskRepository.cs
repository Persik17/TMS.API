using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    /// <summary>
    /// Task-specific repository with additional task-related queries, extending <see cref="IAuditableCommandRepository{Task}"/> and <see cref="IAuditableQueryRepository{Task}"/>.
    /// </summary>
    public interface ITaskRepository :
        IAuditableCommandRepository<DataModels.Task>,
        IAuditableQueryRepository<DataModels.Task>
    {
        Task<List<DataModels.Task>> GetTasksByColumnId(Guid columnId, CancellationToken cancellationToken = default);
        Task<List<DataModels.Task>> GetTasksByColumnIds(IEnumerable<Guid> columnIds, CancellationToken cancellationToken = default);
    }
}
using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    public interface ITaskFileRepository : ICommandRepository<TaskFile>, IQueryRepository<TaskFile>
    {
        Task<List<TaskFile>> GetFilesByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default);
    }
}

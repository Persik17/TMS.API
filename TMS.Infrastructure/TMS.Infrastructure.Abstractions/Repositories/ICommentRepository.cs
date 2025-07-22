using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    public interface ICommentRepository : 
        IAuditableCommandRepository<Comment>, 
        IAuditableQueryRepository<Comment>
    {
        Task<List<Comment>> GetCommentsByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default);
    }
}

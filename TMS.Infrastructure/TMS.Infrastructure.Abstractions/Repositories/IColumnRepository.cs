using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    public interface IColumnRepository :
        IAuditableCommandRepository<Column>,
        IAuditableQueryRepository<Column>
    {
        Task<List<Column>> GetColumnsByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
    }
}

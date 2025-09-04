using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    /// <summary>
    /// Board-specific repository with additional board-related queries, extending <see cref="IAuditableCommandRepository{Board}"/> and <see cref="IAuditableQueryRepository{Board}"/>.
    /// </summary>
    public interface IBoardRepository :
        IAuditableCommandRepository<Board>,
        IAuditableQueryRepository<Board>
    {
        Task<List<Board>> GetBoardsByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
        Task<Board> GetBoardByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
    }
}
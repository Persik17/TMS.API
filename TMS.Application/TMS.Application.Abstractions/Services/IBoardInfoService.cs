using TMS.Application.Dto.Board;

namespace TMS.Application.Abstractions.Services
{
    public interface IBoardInfoService
    {
        Task<BoardSummaryInfoDto?> GetBoardInfoAsync(Guid boardId, Guid userId, Guid companyId, CancellationToken cancellationToken = default);
        Task<BoardAnalyticsDto> GetBoardAnalyticsAsync(Guid boardId, Guid userId, CancellationToken cancellationToken = default);
    }
}

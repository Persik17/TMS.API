using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto;
using TMS.Application.Dto.Board;
using TMS.Application.Dto.User;

namespace TMS.Application.Abstractions.Services
{
    /// <summary>
    /// Service contract for managing Board entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    public interface IBoardService :
        ICreateService<BoardCreateDto, BoardDto>,
        IReadService<BoardDto>,
        IUpdateService<BoardDto>,
        IDeleteService
    {
        Task<List<BoardDto>> GetBoardsByCompanyIdAsync(Guid companyId, Guid userId, CancellationToken cancellationToken = default);
        Task<List<GlobalSearchResultDto>> GlobalSearchTasksAsync(string query, Guid userId, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetUsersByBoardIdAsync(Guid boardId, Guid userId, CancellationToken cancellationToken = default);
    }
}
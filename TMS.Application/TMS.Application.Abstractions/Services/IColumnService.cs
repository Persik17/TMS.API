using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.Column;

namespace TMS.Application.Abstractions.Services
{
    /// <summary>
    /// Service contract for managing Column entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    public interface IColumnService :
        ICreateService<ColumnCreateDto, ColumnDto >,
        IReadService<ColumnDto>,
        IUpdateService<ColumnDto>,
        IDeleteService
    {
        Task<List<ColumnDto>> GetColumnsByBoardIdAsync(Guid boardId, Guid userId, CancellationToken cancellationToken = default);
    }
}
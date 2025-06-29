using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.Board;

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
    }
}
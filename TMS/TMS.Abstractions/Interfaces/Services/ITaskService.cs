using TMS.Abstractions.Interfaces.Services.BaseCommands;
using TMS.Abstractions.Models.DTOs;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing Task entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    /// <typeparam name="TReadModel">DTO or ViewModel used for reading and updating task data.</typeparam>
    /// <typeparam name="TCreateModel">DTO used for creating a new task.</typeparam>
    public interface ITaskService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>,
        IDeleteService
        where TReadModel : class
        where TCreateModel : class
    {
        Task<CommentDto> AddCommentAsync(Guid taskId, CommentCreateDto dto, CancellationToken cancellationToken = default);
        Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid taskId, CancellationToken cancellationToken = default);
        Task DeleteCommentAsync(Guid taskId, Guid commentId, CancellationToken cancellationToken = default);
        Task<CommentDto> UpdateCommentAsync(Guid taskId, Guid commentId, CommentDto dto, CancellationToken cancellationToken = default);
    }
}
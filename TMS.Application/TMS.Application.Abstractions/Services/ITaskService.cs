using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.Comment;
using TMS.Application.Dto.Task;

namespace TMS.Application.Abstractions.Services
{
    /// <summary>
    /// Service contract for managing Task entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    public interface ITaskService :
        ICreateService<TaskCreateDto, CreatedTaskDto>,
        IReadService<TaskDto>,
        IUpdateService<TaskDto>,
        IDeleteService
    {
        Task<List<TaskDto>> GetTasksByColumnId(Guid columnId, CancellationToken cancellationToken = default);
        Task<List<TaskDto>> GetTasksByColumnIds(IEnumerable<Guid> columnIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a comment to a task asynchronously.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task.</param>
        /// <param name="dto">The DTO containing the comment data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous add comment operation. The task result contains the created comment as a DTO.</returns>
        Task<CommentDto> AddCommentAsync(Guid taskId, CommentCreateDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all comments for a task asynchronously.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous get comments operation. The task result contains a collection of comments as DTOs.</returns>
        Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a comment from a task asynchronously.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task.</param>
        /// <param name="commentId">The unique identifier of the comment to delete.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous delete comment operation.</returns>
        Task DeleteCommentAsync(Guid taskId, Guid commentId, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a comment on a task asynchronously.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task.</param>
        /// <param name="commentId">The unique identifier of the comment to update.</param>
        /// <param name="dto">The DTO containing the updated comment data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous update comment operation. The task result contains the updated comment as a DTO.</returns>
        Task<CommentDto> UpdateCommentAsync(Guid taskId, Guid commentId, CommentDto dto, CancellationToken cancellationToken = default);
    }
}
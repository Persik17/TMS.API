using TMS.Abstractions.Interfaces.Services.BaseCommands;
using TMS.Abstractions.Models.DTOs.Task;

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
        Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid taskId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a comment from a task asynchronously.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task.</param>
        /// <param name="commentId">The unique identifier of the comment to delete.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous delete comment operation.</returns>
        Task DeleteCommentAsync(Guid taskId, Guid commentId, CancellationToken cancellationToken = default);

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
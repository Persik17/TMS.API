using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Comment;
using TMS.Application.Dto.Task;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing Task entities.
    /// Provides CRUD operations with logging and validation.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<TaskService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="taskRepository">The repository for accessing task data.</param>
        /// <param name="commentCommandRepository">The repository for performing auditable comment commands (e.g., insert, update).</param>
        /// <param name="commentQueryRepository">The repository for performing auditable comment queries (e.g., get by id).</param>
        /// <param name="logger">The logger for logging task service events.</param>
        public TaskService(
            ITaskRepository taskRepository,
            ICommentRepository commentRepository,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _commentRepository = commentRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<TaskDto> CreateAsync(TaskCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new task: {Name}", createDto.Name);

            var entity = createDto.ToTask();
            await _taskRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("Task created with id: {Id}", entity.Id);

            return entity.ToTaskDto();
        }

        /// <inheritdoc/>
        public async Task<TaskDto> UpdateAsync(TaskDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _taskRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.BoardId = dto.BoardId;
            entity.CreatorId = dto.CreatorId;
            entity.AssigneeId = dto.AssigneeId;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.ActualClosingDate = dto.ActualClosingDate;
            entity.StoryPoints = dto.StoryPoints;
            entity.TaskTypeId = dto.TaskTypeId;
            entity.Priority = dto.Priority;
            entity.Severity = dto.Severity;
            entity.ParentTaskId = dto.ParentTaskId;
            entity.ColumnId = dto.ColumnId;
            entity.UpdateDate = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(entity, cancellationToken);

            _logger.LogInformation("Task with id {Id} updated", dto.Id);

            return entity.ToTaskDto();
        }

        /// <inheritdoc/>
        public async Task<TaskDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _taskRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Task with id {Id} not found", id);
                return null;
            }
            return entity.ToTaskDto();
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            await _taskRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Task with id {Id} deleted", id);
        }

        /// <inheritdoc/>
        public async Task<CommentDto> AddCommentAsync(Guid taskId, CommentCreateDto dto, CancellationToken cancellationToken = default)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                UserId = dto.AuthorId,
                Text = dto.Text,
                CreationDate = DateTime.UtcNow
            };
            await _commentRepository.InsertAsync(comment, cancellationToken);
            return new CommentDto
            {
                Id = comment.Id,
                TaskId = comment.TaskId,
                AuthorId = comment.UserId,
                Text = comment.Text,
                CreationDate = comment.CreationDate
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default)
        {
            var comments = await _commentRepository.GetAllAsync(cancellationToken);
            return [.. comments
                .Where(c => c.TaskId == taskId && c.DeleteDate == null)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    TaskId = c.TaskId,
                    AuthorId = c.UserId,
                    Text = c.Text,
                    CreationDate = c.CreationDate,
                    UpdateDate = c.UpdateDate,
                    DeleteDate = c.DeleteDate
                })];
        }

        /// <inheritdoc/>
        public async Task<CommentDto> UpdateCommentAsync(Guid taskId, Guid commentId, CommentDto dto, CancellationToken cancellationToken = default)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId, cancellationToken);
            if (comment == null || comment.TaskId != taskId)
                throw new NotFoundException(typeof(Comment));

            comment.Text = dto.Text;
            comment.UpdateDate = DateTime.UtcNow;

            await _commentRepository.UpdateAsync(comment, cancellationToken);

            return new CommentDto
            {
                Id = comment.Id,
                TaskId = comment.TaskId,
                AuthorId = comment.UserId,
                Text = comment.Text,
                CreationDate = comment.CreationDate,
                UpdateDate = comment.UpdateDate,
                DeleteDate = comment.DeleteDate
            };
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task DeleteCommentAsync(Guid taskId, Guid commentId, Guid userId, CancellationToken cancellationToken = default)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId, cancellationToken);
            if (comment == null || comment.TaskId != taskId)
                throw new NotFoundException(typeof(Comment));
            await _commentRepository.DeleteAsync(commentId, cancellationToken);
        }
    }
}
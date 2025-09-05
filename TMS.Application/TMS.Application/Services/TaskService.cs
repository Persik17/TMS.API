using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Security;
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
        private readonly IBoardRepository _boardRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskFileRepository _taskFileRepository;
        private readonly IAccessService _accessService;
        private readonly ILogger<TaskService> _logger;

        public TaskService(
            ITaskRepository taskRepository,
            IBoardRepository boardRepository,
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            ITaskFileRepository taskFileRepository,
            IAccessService accessService,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _taskFileRepository = taskFileRepository ?? throw new ArgumentNullException(nameof(taskFileRepository));
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<CreatedTaskDto> CreateAsync(TaskCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            if (!await _accessService.HasPermissionAsync(userId, createDto.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to create tasks on board {BoardId}", userId, createDto.BoardId);
                throw new ForbiddenException();
            }

            _logger.LogInformation("User {UserId} is creating new task: {Name}", userId, createDto.Name);

            var entity = createDto.ToTask();
            await _taskRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("Task created with id: {Id} on board {BoardId}", entity.Id, entity.BoardId);

            return new CreatedTaskDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                BoardId = entity.BoardId,
                Priority = entity.Priority,
                StoryPoints = entity.StoryPoints,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                TaskTypeId = entity.TaskTypeId,
                CreatorId = entity.CreatorId,
                AssigneeId = entity.AssigneeId,
                ColumnId = entity.ColumnId
            };
        }

        /// <inheritdoc/>
        public async Task<TaskUpdateDto> UpdateAsync(TaskUpdateDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _taskRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            if (!await _accessService.HasPermissionAsync(userId, entity.BoardId, ResourceType.Board, cancellationToken))
                throw new ForbiddenException();

            entity.Name = dto.Name ?? entity.Name;
            entity.Description = dto.Description ?? entity.Description;
            entity.AssigneeId = dto.AssigneeId ?? entity.AssigneeId;
            entity.StoryPoints = dto.StoryPoints ?? entity.StoryPoints;
            entity.Priority = dto.Priority ?? entity.Priority;
            entity.Severity = dto.Severity ?? entity.Severity;
            entity.UpdateDate = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(entity, cancellationToken);

            // Возвращаем TaskUpdateDto
            return new TaskUpdateDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                AssigneeId = entity.AssigneeId,
                StoryPoints = entity.StoryPoints,
                Priority = entity.Priority,
                Severity = entity.Severity
            };
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

            if (!await _accessService.HasPermissionAsync(userId, entity.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to view task {TaskId} on board {BoardId}", userId, entity.Id, entity.BoardId);
                throw new ForbiddenException();
            }

            var creatorName = string.Empty;
            var assigneeName = string.Empty;
            if (entity.CreatorId != Guid.Empty)
            {
                var creator = await _userRepository.GetByIdAsync(entity.CreatorId, cancellationToken);
                creatorName = creator?.FullName ?? string.Empty;
            }
            if (entity.AssigneeId.HasValue && entity.AssigneeId.Value != Guid.Empty)
            {
                var assignee = await _userRepository.GetByIdAsync(entity.AssigneeId.Value, cancellationToken);
                assigneeName = string.IsNullOrWhiteSpace(assignee.FullName) ? assignee.Email : assignee.FullName;
            }

            return new TaskDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                BoardId = entity.BoardId,
                CreatorId = entity.CreatorId,
                CreatorName = creatorName,
                AssigneeId = entity.AssigneeId,
                AssigneeName = assigneeName,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                ActualClosingDate = entity.ActualClosingDate,
                StoryPoints = entity.StoryPoints,
                TaskTypeId = entity.TaskTypeId,
                Priority = entity.Priority,
                Severity = entity.Severity,
                ParentTaskId = entity.ParentTaskId,
                ColumnId = entity.ColumnId,
                CreationDate = entity.CreationDate,
                UpdateDate = entity.UpdateDate,
                DeleteDate = entity.DeleteDate
            };
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _taskRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            // Проверка доступа к доске
            if (!await _accessService.HasPermissionAsync(userId, entity.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to delete task {TaskId} on board {BoardId}", userId, id, entity.BoardId);
                throw new ForbiddenException();
            }

            await _taskRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Task with id {Id} deleted by user {UserId}", id, userId);
        }

        /// <inheritdoc/>
        public async Task<CommentDto> AddCommentAsync(Guid taskId, CommentCreateDto dto, CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            // Проверяем только право на доску (любое, например, ViewTask)
            if (!await _accessService.HasPermissionAsync(dto.AuthorId, task.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no access to board {BoardId} to comment on task {TaskId}", dto.AuthorId, task.BoardId, taskId);
                throw new ForbiddenException();
            }

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

        public async Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            if (!await _accessService.HasPermissionAsync(userId, task.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no access to board {BoardId} to view comments for task {TaskId}", userId, task.BoardId, taskId);
                throw new ForbiddenException();
            }

            var comments = await _commentRepository.GetAllAsync(cancellationToken);
            var authorIds = comments
                .Where(c => c.TaskId == taskId && c.DeleteDate == null)
                .Select(c => c.UserId)
                .Distinct()
                .ToList();

            var users = await _userRepository.GetByIdsAsync(authorIds, cancellationToken);
            var userNames = users.ToDictionary(u => u.Id, u => u.FullName);

            return comments
                .Where(c => c.TaskId == taskId && c.DeleteDate == null)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    TaskId = c.TaskId,
                    AuthorId = c.UserId,
                    AuthorName = userNames.TryGetValue(c.UserId, out var name) ? name : "Нет имени",
                    Text = c.Text,
                    CreationDate = c.CreationDate,
                    UpdateDate = c.UpdateDate,
                    DeleteDate = c.DeleteDate
                })
                .ToList();
        }

        public async Task<CommentDto> UpdateCommentAsync(Guid taskId, Guid commentId, CommentDto dto, CancellationToken cancellationToken = default)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId, cancellationToken);
            if (comment == null || comment.TaskId != taskId)
                throw new NotFoundException(typeof(Comment));

            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            // Проверяем только право на доску (любое, например, ViewTask)
            if (!await _accessService.HasPermissionAsync(dto.AuthorId, task.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no access to board {BoardId} to update comment {CommentId} on task {TaskId}", dto.AuthorId, task.BoardId, commentId, taskId);
                throw new ForbiddenException();
            }

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

        public async System.Threading.Tasks.Task DeleteCommentAsync(Guid taskId, Guid commentId, Guid userId, CancellationToken cancellationToken = default)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId, cancellationToken);
            if (comment == null || comment.TaskId != taskId)
                throw new NotFoundException(typeof(Comment));

            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            // Проверяем только право на доску (любое, например, ViewTask)
            if (!await _accessService.HasPermissionAsync(userId, task.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no access to board {BoardId} to delete comment {CommentId} on task {TaskId}", userId, task.BoardId, commentId, taskId);
                throw new ForbiddenException();
            }

            await _commentRepository.DeleteAsync(commentId, cancellationToken);
            _logger.LogInformation("Comment with id {Id} deleted by user {UserId}", commentId, userId);
        }

        public Task<List<TaskDto>> GetTasksByColumnId(Guid columnId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskDto>> GetTasksByColumnIds(IEnumerable<Guid> columnIds, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async System.Threading.Tasks.Task MoveTaskToColumn(Guid taskId, Guid columnId, Guid userId, CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            if (!await _accessService.HasPermissionAsync(userId, task.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to move task {TaskId} on board {BoardId}", userId, task.Id, task.BoardId);
                throw new ForbiddenException();
            }

            task.ColumnId = columnId;
            task.UpdateDate = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task, cancellationToken);

            _logger.LogInformation("Task {TaskId} moved to column {ColumnId} by user {UserId}", taskId, columnId, userId);
        }

        public async Task<List<TaskDto>> GetTasksByAssigneeIdAsync(Guid assigneeId, Guid userId, CancellationToken cancellationToken = default)
        {
            var tasks = await _taskRepository.GetTasksByAssigneeIdAsync(assigneeId, cancellationToken);

            var userIds = tasks
                .Select(t => t.CreatorId)
                .Concat(tasks.Where(t => t.AssigneeId.HasValue).Select(t => t.AssigneeId.Value))
                .Distinct()
                .ToList();

            var boardIds = tasks.Select(t => t.BoardId).Distinct().ToList();

            var users = await _userRepository.GetAllAsync(cancellationToken);
            var userDict = users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id, u => u.FullName ?? string.Empty);

            var boards = await _boardRepository.GetByIdsAsync(boardIds, cancellationToken);
            var boardDict = boards.ToDictionary(b => b.Id, b => b.Name);

            return tasks.Select(entity => new TaskDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                BoardId = entity.BoardId,
                BoardName = boardDict.GetValueOrDefault(entity.BoardId, string.Empty),
                CreatorId = entity.CreatorId,
                CreatorName = userDict.GetValueOrDefault(entity.CreatorId, string.Empty),
                AssigneeId = entity.AssigneeId,
                AssigneeName = entity.AssigneeId.HasValue ? userDict.GetValueOrDefault(entity.AssigneeId.Value, string.Empty) : string.Empty,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                ActualClosingDate = entity.ActualClosingDate,
                StoryPoints = entity.StoryPoints,
                TaskTypeId = entity.TaskTypeId,
                Priority = entity.Priority,
                Severity = entity.Severity,
                ParentTaskId = entity.ParentTaskId,
                ColumnId = entity.ColumnId,
                CreationDate = entity.CreationDate,
                UpdateDate = entity.UpdateDate,
                DeleteDate = entity.DeleteDate
            }).ToList();
        }

        public async Task<List<TaskFileDto>> GetFilesAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            if (!await _accessService.HasPermissionAsync(userId, task.BoardId, ResourceType.Board, cancellationToken))
                throw new ForbiddenException();

            var files = await _taskFileRepository.GetFilesByTaskIdAsync(taskId, cancellationToken);
            return [.. files.Select(f => new TaskFileDto
            {
                Id = f.Id,
                TaskId = f.TaskId,
                FileName = f.FileName,
                ContentType = f.ContentType,
                CreationDate = f.CreationDate,
            })];
        }

        public async System.Threading.Tasks.Task DeleteFileAsync(Guid taskId, Guid fileId, Guid userId, CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            if (!await _accessService.HasPermissionAsync(userId, task.BoardId, ResourceType.Board, cancellationToken))
                throw new ForbiddenException();

            var file = await _taskFileRepository.GetByIdAsync(fileId, cancellationToken);
            if (file == null || file.TaskId != taskId)
                throw new NotFoundException(typeof(TaskFile));

            await _taskFileRepository.DeleteAsync(fileId, cancellationToken);
        }

        public async Task<TaskFileDto> DownloadFileAsync(Guid taskId, Guid fileId, Guid userId, CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            if (!await _accessService.HasPermissionAsync(userId, task.BoardId, ResourceType.Board, cancellationToken))
                throw new ForbiddenException();

            var file = await _taskFileRepository.GetByIdAsync(fileId, cancellationToken);
            if (file == null || file.TaskId != taskId)
                throw new NotFoundException(typeof(TaskFile));

            return new TaskFileDto
            {
                Id = file.Id,
                TaskId = file.TaskId,
                FileName = file.FileName,
                ContentType = file.ContentType,
                CreationDate = file.CreationDate,
                FileData = file.FileData
            };
        }

        public async Task<TaskFileDto> UploadFileAsync(
            Guid taskId,
            IFormFile file,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("UploadFileAsync called. taskId={TaskId}, userId={UserId}, file={FileName}, fileLength={FileLength}",
                    taskId, userId, file?.FileName, file?.Length ?? -1);

                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("UploadFileAsync: файл не передан или пустой. file={File}", file);
                    throw new ArgumentException("Файл не передан или пустой", nameof(file));
                }

                var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);
                if (task == null)
                {
                    _logger.LogWarning("UploadFileAsync: Task not found. taskId={TaskId}", taskId);
                    throw new NotFoundException(typeof(Infrastructure.DataModels.Task));
                }

                bool hasPermission = await _accessService.HasPermissionAsync(userId, task.BoardId, ResourceType.Board, cancellationToken);
                if (!hasPermission)
                {
                    _logger.LogWarning("UploadFileAsync: Нет прав на upload. userId={UserId}, taskId={TaskId}, boardId={BoardId}", userId, taskId, task.BoardId);
                    throw new ForbiddenException();
                }

                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms, cancellationToken);
                    fileBytes = ms.ToArray();
                }

                var newFile = new TaskFile
                {
                    Id = Guid.NewGuid(),
                    TaskId = taskId,
                    FileName = file.FileName,
                    ContentType = file.ContentType ?? "application/octet-stream",
                    CreationDate = DateTime.UtcNow,
                    FileData = fileBytes
                };

                await _taskFileRepository.InsertAsync(newFile, cancellationToken);

                _logger.LogInformation("UploadFileAsync: User {UserId} uploaded file {FileName} (id: {FileId}) to task {TaskId}",
                    userId, newFile.FileName, newFile.Id, taskId);

                return new TaskFileDto
                {
                    Id = newFile.Id,
                    TaskId = newFile.TaskId,
                    FileName = newFile.FileName,
                    ContentType = newFile.ContentType,
                    CreationDate = newFile.CreationDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в UploadFileAsync! taskId={TaskId}, userId={UserId}, file={FileName}", taskId, userId, file?.FileName);
                throw;
            }
        }
    }
}
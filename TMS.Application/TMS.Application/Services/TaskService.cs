﻿using Microsoft.Extensions.Logging;
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
        private readonly ICommentRepository _commentRepository;
        private readonly IAccessService _accessService;
        private readonly ILogger<TaskService> _logger;

        public TaskService(
            ITaskRepository taskRepository,
            ICommentRepository commentRepository,
            IAccessService accessService,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<TaskDto> CreateAsync(TaskCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            // Проверка доступа к доске
            if (!await _accessService.HasPermissionAsync(userId, createDto.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to create tasks on board {BoardId}", userId, createDto.BoardId);
                throw new ForbiddenException();
            }

            _logger.LogInformation("User {UserId} is creating new task: {Name}", userId, createDto.Name);

            var entity = createDto.ToTask();
            await _taskRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("Task created with id: {Id} on board {BoardId}", entity.Id, entity.BoardId);

            return entity.ToTaskDto();
        }

        /// <inheritdoc/>
        public async Task<TaskDto> UpdateAsync(TaskDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _taskRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Infrastructure.DataModels.Task));

            // Проверка доступа к доске
            if (!await _accessService.HasPermissionAsync(userId, entity.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to update task {TaskId} on board {BoardId}", userId, entity.Id, entity.BoardId);
                throw new ForbiddenException();
            }

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

            _logger.LogInformation("Task with id {Id} updated by user {UserId}", dto.Id, userId);

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

            // Проверка доступа к доске
            if (!await _accessService.HasPermissionAsync(userId, entity.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to view task {TaskId} on board {BoardId}", userId, entity.Id, entity.BoardId);
                throw new ForbiddenException();
            }

            return entity.ToTaskDto();
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

            // Проверяем только право на доску (любое, например, ViewTask)
            if (!await _accessService.HasPermissionAsync(userId, task.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no access to board {BoardId} to view comments for task {TaskId}", userId, task.BoardId, taskId);
                throw new ForbiddenException();
            }

            var comments = await _commentRepository.GetAllAsync(cancellationToken);
            return comments
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
    }
}
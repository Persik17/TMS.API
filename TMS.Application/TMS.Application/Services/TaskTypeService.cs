using Microsoft.Extensions.Logging;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Security;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.TaskType;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing TaskType entities.
    /// Provides CRUD operations with logging and validation.
    /// </summary>
    public class TaskTypeService : ITaskTypeService
    {
        private readonly ITaskTypeRepository _taskTypeRepository;
        private readonly IAccessService _accessService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<TaskTypeService> _logger;

        private static readonly TimeSpan TaskTypeCacheExpiry = TimeSpan.FromDays(30);

        public TaskTypeService(
            ITaskTypeRepository taskTypeRepository,
            IAccessService accessService,
            ICacheService cacheService,
            ILogger<TaskTypeService> logger)
        {
            _taskTypeRepository = taskTypeRepository ?? throw new ArgumentNullException(nameof(taskTypeRepository));
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TaskTypeDto> CreateAsync(TaskTypeCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            if (!await _accessService.HasPermissionAsync(userId, Guid.Empty, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to create task type on board {BoardId}", userId, Guid.Empty);
                throw new ForbiddenException();
            }

            _logger.LogInformation("Creating new task type: {Name}", createDto.Name);

            var entity = createDto.ToTaskType();
            await _taskTypeRepository.InsertAsync(entity, cancellationToken);

            var dto = entity.ToTaskTypeDto();
            await _cacheService.SetAsync(CacheKeys.TaskType(entity.Id), dto, TaskTypeCacheExpiry);

            _logger.LogInformation("TaskType created with id: {Id}", entity.Id);

            return dto;
        }

        public async Task<TaskTypeDto> UpdateAsync(TaskTypeDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _taskTypeRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(TaskType));

            if (!await _accessService.HasPermissionAsync(userId, entity.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to update task type {TaskTypeId} on board {BoardId}", userId, dto.Id, entity.BoardId);
                throw new ForbiddenException();
            }

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.UpdateDate = DateTime.UtcNow;

            await _taskTypeRepository.UpdateAsync(entity, cancellationToken);

            var updatedDto = entity.ToTaskTypeDto();
            await _cacheService.SetAsync(CacheKeys.TaskType(dto.Id), updatedDto, TaskTypeCacheExpiry);

            _logger.LogInformation("TaskType with id {Id} updated", dto.Id);

            return updatedDto;
        }

        public async Task<TaskTypeDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var cacheKey = CacheKeys.TaskType(id);
            var cached = await _cacheService.GetAsync<TaskTypeDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogDebug("TaskType with id {Id} found in cache", id);
                return cached;
            }

            var entity = await _taskTypeRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("TaskType with id {Id} not found", id);
                return null;
            }

            if (!await _accessService.HasPermissionAsync(userId, entity.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to view task type {TaskTypeId} on board {BoardId}", userId, id, entity.BoardId);
                throw new ForbiddenException();
            }

            var dto = entity.ToTaskTypeDto();
            await _cacheService.SetAsync(cacheKey, dto, TaskTypeCacheExpiry);

            return dto;
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _taskTypeRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(typeof(TaskType));

            if (!await _accessService.HasPermissionAsync(userId, entity.BoardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to delete task type {TaskTypeId} on board {BoardId}", userId, id, entity.BoardId);
                throw new ForbiddenException();
            }

            await _taskTypeRepository.DeleteAsync(id, cancellationToken);

            await _cacheService.RemoveAsync(CacheKeys.TaskType(id));

            _logger.LogInformation("TaskType with id {Id} deleted", id);
        }

        public async Task<List<TaskTypeDto>> GetTasksByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
        {
            var entities = await _taskTypeRepository.GetTaskTypesByBoardIdAsync(boardId, cancellationToken);
            return entities.Select(x => x.ToTaskTypeDto()).ToList();
        }
    }
}
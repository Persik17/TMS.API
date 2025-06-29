using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
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
        private readonly ILogger<TaskTypeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeService"/> class.
        /// </summary>
        /// <param name="commandRepository">The repository for performing auditable task type commands (e.g., insert, update).</param>
        /// <param name="queryRepository">The repository for performing auditable task type queries (e.g., get by id).</param>
        /// <param name="logger">The logger for logging task type service events.</param>
        public TaskTypeService(
            ITaskTypeRepository taskTypeRepository,
            ILogger<TaskTypeService> logger)
        {
            _taskTypeRepository = taskTypeRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<TaskTypeDto> CreateAsync(TaskTypeCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new task type: {Name}", createDto.Name);

            var entity = createDto.ToTaskType();
            await _taskTypeRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("TaskType created with id: {Id}", entity.Id);

            return entity.ToTaskTypeDto();
        }

        /// <inheritdoc/>
        public async Task<TaskTypeDto> UpdateAsync(TaskTypeDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _taskTypeRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(TaskType));

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.UpdateDate = DateTime.UtcNow;

            await _taskTypeRepository.UpdateAsync(entity, cancellationToken);

            _logger.LogInformation("TaskType with id {Id} updated", dto.Id);

            return entity.ToTaskTypeDto();
        }

        /// <inheritdoc/>
        public async Task<TaskTypeDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _taskTypeRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("TaskType with id {Id} not found", id);
                return null;
            }
            return entity.ToTaskTypeDto();
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            await _taskTypeRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("TaskType with id {Id} deleted", id);
        }
    }
}
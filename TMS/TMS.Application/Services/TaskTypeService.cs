using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.DTOs.TaskType;
using TMS.Application.Extensions;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing TaskType entities.
    /// Provides CRUD operations with logging and validation.
    /// </summary>
    public class TaskTypeService : ITaskTypeService<TaskTypeDto, TaskTypeCreateDto>
    {
        private readonly IAuditableCommandRepository<TaskType> _commandRepository;
        private readonly IAuditableQueryRepository<TaskType> _queryRepository;
        private readonly ILogger<TaskTypeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeService"/> class.
        /// </summary>
        /// <param name="commandRepository">The repository for performing auditable task type commands (e.g., insert, update).</param>
        /// <param name="queryRepository">The repository for performing auditable task type queries (e.g., get by id).</param>
        /// <param name="logger">The logger for logging task type service events.</param>
        public TaskTypeService(
            IAuditableCommandRepository<TaskType> commandRepository,
            IAuditableQueryRepository<TaskType> queryRepository,
            ILogger<TaskTypeService> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<TaskTypeDto> CreateAsync(TaskTypeCreateDto createDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new task type: {Name}", createDto.Name);

            var entity = createDto.ToTaskType();
            await _commandRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("TaskType created with id: {Id}", entity.Id);

            return entity.ToTaskTypeDto();
        }

        /// <inheritdoc/>
        public async Task<TaskTypeDto> UpdateAsync(TaskTypeDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _queryRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(TaskType));

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.UpdateDate = DateTime.UtcNow;

            await _commandRepository.UpdateAsync(entity, cancellationToken);

            _logger.LogInformation("TaskType with id {Id} updated", dto.Id);

            return entity.ToTaskTypeDto();
        }

        /// <inheritdoc/>
        public async Task<TaskTypeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _queryRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("TaskType with id {Id} not found", id);
                return null;
            }
            return entity.ToTaskTypeDto();
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _commandRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("TaskType with id {Id} deleted", id);
        }
    }
}
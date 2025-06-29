using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Permission;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing Permission entities.
    /// Provides create, update, and get operations with logging and validation.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<PermissionService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionService"/> class.
        /// </summary>
        /// <param name="commandRepository">The repository for performing auditable permission commands (e.g., insert, update).</param>
        /// <param name="queryRepository">The repository for performing auditable permission queries (e.g., get by id).</param>
        /// <param name="logger">The logger for logging permission service events.</param>
        public PermissionService(
            IPermissionRepository permissionRepository,
            ILogger<PermissionService> logger)
        {
            _permissionRepository = permissionRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<PermissionDto> CreateAsync(PermissionCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new permission: {Name}", createDto.Name);

            var entity = createDto.ToPermission();
            await _permissionRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("Permission created with id: {Id}", entity.Id);

            return entity.ToPermissionDto();
        }

        /// <inheritdoc/>
        public async Task<PermissionDto> UpdateAsync(PermissionDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _permissionRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Permission));

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.UpdateDate = DateTime.UtcNow;

            await _permissionRepository.UpdateAsync(entity, cancellationToken);

            _logger.LogInformation("Permission with id {Id} updated", dto.Id);

            return entity.ToPermissionDto();
        }

        /// <inheritdoc/>
        public async Task<PermissionDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _permissionRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Permission with id {Id} not found", id);
                return null;
            }
            return entity.ToPermissionDto();
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete department with empty id");
                throw new WrongIdException(typeof(Department));
            }

            await _permissionRepository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Department with id {Id} deleted", id);
        }
    }
}
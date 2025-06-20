using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.DTOs.Department;
using TMS.Application.Extensions;
using TMS.Infrastructure.DataAccess.DataModels;

using Task = System.Threading.Tasks.Task;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing Department entities.
    /// Provides CRUD operations and uses logging for diagnostics and monitoring.
    /// </summary>
    public class DepartmentService : IDepartmentService<DepartmentDto, DepartmentCreateDto>
    {
        private readonly IAuditableCommandRepository<Department> _commandRepository;
        private readonly IAuditableQueryRepository<Department> _queryRepository;
        private readonly ILogger<DepartmentService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentService"/> class.
        /// </summary>
        /// <param name="commandRepository">The repository for performing auditable department commands (e.g., insert, update).</param>
        /// <param name="queryRepository">The repository for performing auditable department queries (e.g., get by id).</param>
        /// <param name="logger">The logger for logging department service events.</param>
        public DepartmentService(
            IAuditableCommandRepository<Department> commandRepository,
            IAuditableQueryRepository<Department> queryRepository,
            ILogger<DepartmentService> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<DepartmentDto> CreateAsync(DepartmentCreateDto createDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new department: {Name}", createDto.Name);

            var newDepartment = createDto.ToDepartment();
            newDepartment.Id = Guid.NewGuid();

            await _commandRepository.InsertAsync(newDepartment, cancellationToken);

            var createdDepartment = await _queryRepository.GetByIdAsync(newDepartment.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Department));

            _logger.LogInformation("Department created successfully with id: {Id}", newDepartment.Id);

            return createdDepartment.ToDepartmentDto();
        }

        /// <inheritdoc/>
        public async Task<DepartmentDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get department with empty id");
                throw new WrongIdException(typeof(Department));
            }

            var department = await _queryRepository.GetByIdAsync(id, cancellationToken);
            if (department == null)
                _logger.LogWarning("Department with id {Id} not found", id);

            return department?.ToDepartmentDto();
        }

        /// <inheritdoc/>
        public async Task<DepartmentDto> UpdateAsync(DepartmentDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.Id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to update department with empty id");
                throw new WrongIdException(typeof(Department));
            }

            var existingDepartment = await _queryRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existingDepartment == null)
            {
                _logger.LogWarning("Department with id {Id} not found for update", dto.Id);
                throw new NotFoundException(typeof(Department));
            }

            existingDepartment.UpdateFromDto(dto);

            await _commandRepository.UpdateAsync(existingDepartment, cancellationToken);

            _logger.LogInformation("Department with id {Id} updated successfully", dto.Id);

            return dto;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete department with empty id");
                throw new WrongIdException(typeof(Department));
            }

            await _commandRepository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Department with id {Id} deleted", id);
        }
    }
}
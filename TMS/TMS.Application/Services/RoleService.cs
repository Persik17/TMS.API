using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.Extensions;
using TMS.Application.Models.DTOs.Role;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing Role entities.
    /// Provides CRUD operations with logging and validation.
    /// </summary>
    public class RoleService : IRoleService<RoleDto, RoleCreateDto>
    {
        private readonly IRoleRepository<Role> _roleRepository;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            IRoleRepository<Role> roleRepository,
            ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        public async Task<RoleDto> CreateAsync(RoleCreateDto createDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new role: {Name}", createDto.Name);

            var entity = createDto.ToRole();
            await _roleRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("Role created with id: {Id}", entity.Id);

            return entity.ToRoleDto();
        }

        public async Task<RoleDto> UpdateAsync(RoleDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _roleRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Role));

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.UpdateDate = DateTime.UtcNow;

            await _roleRepository.UpdateAsync(entity, cancellationToken);

            _logger.LogInformation("Role with id {Id} updated", dto.Id);

            return entity.ToRoleDto();
        }

        public async Task<RoleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _roleRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Role with id {Id} not found", id);
                return null;
            }
            return entity.ToRoleDto();
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _roleRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Role with id {Id} deleted", id);
        }
    }
}
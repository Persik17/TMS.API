using Microsoft.Extensions.Logging;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Security;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Role;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing Role entities.
    /// Provides CRUD operations with logging and validation.
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IAccessService _accessService;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            IRoleRepository roleRepository,
            IAccessService accessService,
            ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<RoleDto> CreateAsync(RoleCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            // Проверка доступа: только админ компании или суперпользователь может создавать роли
            if (!await _accessService.HasPermissionAsync(userId, createDto.CompanyId, ResourceType.Company, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to create roles in company {CompanyId}", userId, createDto.CompanyId);
                throw new ForbiddenException();
            }

            _logger.LogInformation("User {UserId} is creating new role: {Name}", userId, createDto.Name);

            var entity = createDto.ToRole();
            await _roleRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("Role created with id: {Id} by user {UserId}", entity.Id, userId);

            return entity.ToRoleDto();
        }

        /// <inheritdoc/>
        public async Task<RoleDto> UpdateAsync(RoleDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _roleRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Role));

            // Проверка доступа: только админ компании или суперпользователь может обновлять роли
            //if (!await _accessService.HasPermissionAsync(userId, entity.CompanyId, ResourceType.Company, PermissionNames.Company.ManageRoles, cancellationToken))
            //{
            //    _logger.LogWarning("User {UserId} has no permission to update role {RoleId} in company {CompanyId}", userId, dto.Id, entity.CompanyId);
            //    throw new ForbiddenException(PermissionNames.Company.ManageRoles);
            //}

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.UpdateDate = DateTime.UtcNow;

            await _roleRepository.UpdateAsync(entity, cancellationToken);

            _logger.LogInformation("Role with id {Id} updated by user {UserId}", dto.Id, userId);

            return entity.ToRoleDto();
        }

        /// <inheritdoc/>
        public async Task<RoleDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _roleRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Role with id {Id} not found", id);
                return null;
            }

            // Если нужно ограничить просмотр — раскомментируй:
            // if (!await _accessService.HasPermissionAsync(userId, entity.CompanyId, ResourceType.Company, PermissionNames.Company.ViewRoles, cancellationToken))
            // {
            //     _logger.LogWarning("User {UserId} has no permission to view role {RoleId} in company {CompanyId}", userId, id, entity.CompanyId);
            //     throw new ForbiddenException(PermissionNames.Company.ViewRoles);
            // }

            return entity.ToRoleDto();
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _roleRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(typeof(Role));

            // Проверка доступа: только админ компании или суперпользователь может удалять роли
            //if (!await _accessService.HasPermissionAsync(userId, entity.CompanyId, ResourceType.Company, PermissionNames.Company.ManageRoles, cancellationToken))
            //{
            //    _logger.LogWarning("User {UserId} has no permission to delete role {RoleId} in company {CompanyId}", userId, id, entity.CompanyId);
            //    throw new ForbiddenException(PermissionNames.Company.ManageRoles);
            //}

            await _roleRepository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Role with id {Id} deleted by user {UserId}", id, userId);
        }
    }
}
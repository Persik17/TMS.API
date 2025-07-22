using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Security;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.User;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Admin service for managing users: full CRUD and invite by email.
    /// </summary>
    public class UserAdminService : IUserAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IInvitationService _invitationService;
        private readonly ICacheService _cacheService;
        private readonly IAccessService _accessService;
        private readonly ILogger<UserAdminService> _logger;

        private static readonly TimeSpan UserCacheExpiry = TimeSpan.FromDays(30);

        public UserAdminService(
            IUserRepository userRepository,
            IInvitationService invitationService,
            ICacheService cacheService,
            IAccessService accessService,
            ILogger<UserAdminService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _invitationService = invitationService ?? throw new ArgumentNullException(nameof(invitationService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a new user (admin only).
        /// </summary>
        public async Task<UserDto> CreateAsync(UserCreateDto createDto, Guid adminId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            // Admin permission check
            if (!await _accessService.HasPermissionAsync(adminId, Guid.Empty, ResourceType.User, cancellationToken))
            {
                _logger.LogWarning("Admin {AdminId} has no permission to create users", adminId);
                throw new ForbiddenException();
            }

            _logger.LogInformation("Admin {AdminId} is creating new user: {FullName}", adminId, createDto.FullName);

            var newUser = createDto.ToUser();
            await _userRepository.InsertAsync(newUser, cancellationToken);

            var createdUser = await _userRepository.GetByIdAsync(newUser.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(User));

            var dto = createdUser.ToUserDto();
            await _cacheService.SetAsync(CacheKeys.User(newUser.Id), dto, UserCacheExpiry);

            _logger.LogInformation("User created by admin {AdminId} with id: {Id}", adminId, newUser.Id);

            return dto;
        }

        /// <summary>
        /// Gets a user by id (admin only).
        /// </summary>
        public async Task<UserDto> GetByIdAsync(Guid id, Guid adminId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get user with empty id");
                throw new WrongIdException(typeof(User));
            }

            // Admin permission check
            if (!await _accessService.HasPermissionAsync(adminId, id, ResourceType.User, cancellationToken))
            {
                _logger.LogWarning("Admin {AdminId} has no permission to view user {TargetId}", adminId, id);
                throw new ForbiddenException();
            }

            var cacheKey = CacheKeys.User(id);
            var cached = await _cacheService.GetAsync<UserDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogDebug("User with id {Id} found in cache", id);
                return cached;
            }

            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User with id {Id} not found", id);
                return null;
            }

            var dto = user.ToUserDto();
            await _cacheService.SetAsync(cacheKey, dto, UserCacheExpiry);

            return dto;
        }

        /// <summary>
        /// Updates a user (admin only).
        /// </summary>
        public async Task<UserDto> UpdateAsync(UserDto dto, Guid adminId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.Id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to update user with empty id");
                throw new WrongIdException(typeof(User));
            }

            // Admin permission check
            if (!await _accessService.HasPermissionAsync(adminId, dto.Id, ResourceType.User, cancellationToken))
            {
                _logger.LogWarning("Admin {AdminId} has no permission to update user {TargetId}", adminId, dto.Id);
                throw new ForbiddenException();
            }

            var existingUser = await _userRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existingUser == null)
            {
                _logger.LogWarning("User with id {Id} not found for update", dto.Id);
                throw new NotFoundException(typeof(User));
            }

            // Admin can update all fields
            existingUser.FullName = dto.FullName;
            existingUser.Email = dto.Email;
            existingUser.TelegramId = dto.TelegramId;
            existingUser.Timezone = dto.Timezone;
            existingUser.Language = dto.Language;
            existingUser.Phone = dto.Phone;
            existingUser.Status = dto.Status;
            existingUser.NotificationSettingsId = dto.NotificationSettingsId;
            existingUser.LastLoginDate = dto.LastLoginDate;
            existingUser.CreationDate = dto.CreationDate;
            existingUser.UpdateDate = DateTime.UtcNow;
            existingUser.DeleteDate = dto.DeleteDate;

            await _userRepository.UpdateAsync(existingUser, cancellationToken);

            var updatedDto = existingUser.ToUserDto();
            await _cacheService.SetAsync(CacheKeys.User(dto.Id), updatedDto, UserCacheExpiry);

            _logger.LogInformation("User with id {Id} updated by admin {AdminId}", dto.Id, adminId);

            return updatedDto;
        }

        /// <summary>
        /// Deletes a user (admin only).
        /// </summary>
        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid adminId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete user with empty id");
                throw new WrongIdException(typeof(User));
            }

            // Admin permission check
            if (!await _accessService.HasPermissionAsync(adminId, id, ResourceType.User, cancellationToken))
            {
                _logger.LogWarning("Admin {AdminId} has no permission to delete user {TargetId}", adminId, id);
                throw new ForbiddenException();
            }

            await _userRepository.DeleteAsync(id, cancellationToken);

            await _cacheService.RemoveAsync(CacheKeys.User(id));

            _logger.LogInformation("User with id {Id} deleted by admin {AdminId}", id, adminId);
        }

        /// <summary>
        /// Invites a user by email (admin only, invitation is sent via IInvitationService).
        /// </summary>
        public async System.Threading.Tasks.Task InviteByEmailAsync(UserInviteDto dto, Guid adminId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            // Admin permission check
            if (!await _accessService.HasPermissionAsync(adminId, Guid.Empty, ResourceType.User, cancellationToken))
            {
                _logger.LogWarning("Admin {AdminId} has no permission to invite users", adminId);
                throw new ForbiddenException();
            }

            _logger.LogInformation("Admin {AdminId} is inviting user by email {Email}", adminId, dto.Email);

            await _invitationService.InviteByEmailAsync(dto, adminId, cancellationToken);
        }
    }
}

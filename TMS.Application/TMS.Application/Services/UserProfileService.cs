using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.TelegramAccount;
using TMS.Application.Dto.User;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

using Task = System.Threading.Tasks.Task;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing User entities.
    /// Provides CRUD operations and uses logging for diagnostics and monitoring.
    /// </summary>
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITelegramAccountRepository _telegramAccountRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<UserProfileService> _logger;

        private static readonly TimeSpan UserCacheExpiry = TimeSpan.FromDays(30);

        public UserProfileService(
            IUserRepository userRepository,
            ITelegramAccountRepository telegramAccountRepository,
            ICacheService cacheService,
            ILogger<UserProfileService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _telegramAccountRepository = telegramAccountRepository ?? throw new ArgumentNullException(nameof(telegramAccountRepository));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            if (id != userId)
            {
                _logger.LogWarning("User {UserId} tried to get profile of another user {Id}", userId, id);
                throw new ForbiddenException();
            }

            var cacheKey = CacheKeys.User(id);
            var cached = await _cacheService.GetAsync<UserDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogDebug("User profile {Id} found in cache", id);
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

        public async Task<UserDto> UpdateAsync(UserDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.Id != userId)
            {
                _logger.LogWarning("User {UserId} tried to update profile of another user {Id}", userId, dto.Id);
                throw new ForbiddenException();
            }

            var existingUser = await _userRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(User));

            existingUser.FullName = dto.FullName;
            existingUser.Phone = dto.Phone;
            existingUser.UpdateDate = DateTime.UtcNow;

            await _userRepository.UpdateAsync(existingUser, cancellationToken);

            var updatedDto = existingUser.ToUserDto();
            await _cacheService.SetAsync(CacheKeys.User(dto.Id), updatedDto, UserCacheExpiry);

            _logger.LogInformation("User {UserId} updated their profile", userId);

            return updatedDto;
        }

        public async Task LinkTelegramAccountAsync(Guid userId, TelegramAccountCreateDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException(typeof(User));

            if (user.TelegramId != null)
            {
                _logger.LogWarning("User {UserId} already has a linked TelegramAccount", userId);
                throw new InvalidOperationException("Telegram account already linked.");
            }

            var telegramAccount = new TelegramAccount
            {
                Id = Guid.NewGuid(),
                Username = dto.NickName,
                Phone = dto.Phone,
                CreationDate = DateTime.UtcNow
            };

            await _telegramAccountRepository.InsertAsync(telegramAccount, cancellationToken);
            user.TelegramId = telegramAccount.Id;
            await _userRepository.UpdateAsync(user, cancellationToken);

            await _cacheService.RemoveAsync(CacheKeys.User(userId));

            _logger.LogInformation("TelegramAccount {TelegramId} linked to user {UserId}", telegramAccount.Id, userId);
        }

        public async Task UnlinkTelegramAccountAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException(typeof(User));

            if (user.TelegramId == null)
            {
                _logger.LogWarning("User {UserId} has no linked TelegramAccount", userId);
                throw new InvalidOperationException("No Telegram account linked.");
            }

            var telegramId = user.TelegramId.Value;

            await _telegramAccountRepository.DeleteAsync(telegramId, cancellationToken);

            user.TelegramId = null;
            await _userRepository.UpdateAsync(user, cancellationToken);

            await _cacheService.RemoveAsync(CacheKeys.User(userId));

            _logger.LogInformation("TelegramAccount {TelegramId} unlinked from user {UserId}", telegramId, userId);
        }
    }
}
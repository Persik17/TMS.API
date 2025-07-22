using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.NotificationSetting;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing NotificationSetting entities.
    /// Provides create, update, and get operations with logging and validation.
    /// </summary>
    public class NotificationSettingService : INotificationSettingService
    {
        private readonly INotificationSettingRepository _settingRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<NotificationSettingService> _logger;

        private static readonly TimeSpan NotificationSettingCacheExpiry = TimeSpan.FromDays(30);

        public NotificationSettingService(
            INotificationSettingRepository settingRepository,
            IUserRepository userRepository,
            ICacheService cacheService,
            ILogger<NotificationSettingService> logger)
        {
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<NotificationSettingDto> CreateAsync(NotificationSettingCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            _logger.LogInformation("User {UserId} is creating new notification setting", userId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException(typeof(User));

            if (user.NotificationSettingsId != null)
            {
                _logger.LogWarning("User {UserId} already has notification settings", userId);
                throw new InvalidOperationException("User already has notification settings.");
            }

            var entity = createDto.ToNotificationSetting();
            entity.Id = Guid.NewGuid();

            await _settingRepository.InsertAsync(entity, cancellationToken);

            user.NotificationSettingsId = entity.Id;
            user.NotificationSettings = entity;
            await _userRepository.UpdateAsync(user, cancellationToken);

            var dto = entity.ToNotificationSettingDto();
            await _cacheService.SetAsync(CacheKeys.NotificationSetting(entity.Id), dto, NotificationSettingCacheExpiry);

            _logger.LogInformation("Notification setting created with id: {Id} for user {UserId}", entity.Id, userId);

            return dto;
        }

        public async Task<NotificationSettingDto> UpdateAsync(NotificationSettingDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException(typeof(User));

            if (user.NotificationSettingsId != dto.Id)
            {
                _logger.LogWarning("User {UserId} tried to update notification setting {SettingId} not belonging to them", userId, dto.Id);
                throw new ForbiddenException();
            }

            var entity = await _settingRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(NotificationSetting));

            entity.EmailNotificationsEnabled = dto.EmailNotificationsEnabled;
            entity.PushNotificationsEnabled = dto.PushNotificationsEnabled;
            entity.TelegramNotificationsEnabled = dto.TelegramNotificationsEnabled;

            await _settingRepository.UpdateAsync(entity, cancellationToken);

            var updatedDto = entity.ToNotificationSettingDto();
            await _cacheService.SetAsync(CacheKeys.NotificationSetting(dto.Id), updatedDto, NotificationSettingCacheExpiry);

            _logger.LogInformation("Notification setting with id {Id} updated by user {UserId}", dto.Id, userId);

            return updatedDto;
        }

        public async Task<NotificationSettingDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException(typeof(User));

            if (user.NotificationSettingsId != id)
            {
                _logger.LogWarning("User {UserId} tried to access notification setting {SettingId} not belonging to them", userId, id);
                throw new ForbiddenException();
            }

            var cacheKey = CacheKeys.NotificationSetting(id);
            var cached = await _cacheService.GetAsync<NotificationSettingDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogDebug("Notification setting with id {Id} found in cache", id);
                return cached;
            }

            var entity = await _settingRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Notification setting with id {Id} not found", id);
                return null;
            }

            var dto = entity.ToNotificationSettingDto();
            await _cacheService.SetAsync(cacheKey, dto, NotificationSettingCacheExpiry);

            return dto;
        }
    }
}
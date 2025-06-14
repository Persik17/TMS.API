using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.Extensions;
using TMS.Application.Models.DTOs.NotificationSetting;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing NotificationSetting entities.
    /// Provides create, update, and get operations with logging and validation.
    /// </summary>
    public class NotificationSettingService : INotificationSettingService<NotificationSettingDto, NotificationSettingCreateDto>
    {
        private readonly ICommandRepository<NotificationSetting> _commandRepository;
        private readonly IQueryRepository<NotificationSetting> _queryRepository;
        private readonly ILogger<NotificationSettingService> _logger;

        public NotificationSettingService(
            ICommandRepository<NotificationSetting> commandRepository,
            IQueryRepository<NotificationSetting> queryRepository,
            ILogger<NotificationSettingService> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        public async Task<NotificationSettingDto> CreateAsync(NotificationSettingCreateDto createDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new notification setting");

            var entity = createDto.ToNotificationSetting();
            await _commandRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("Notification setting created with id: {Id}", entity.Id);

            return entity.ToNotificationSettingDto();
        }

        public async Task<NotificationSettingDto> UpdateAsync(NotificationSettingDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _queryRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(NotificationSetting));

            entity.EmailNotificationsEnabled = dto.EmailNotificationsEnabled;
            entity.PushNotificationsEnabled = dto.PushNotificationsEnabled;
            entity.TelegramNotificationsEnabled = dto.TelegramNotificationsEnabled;

            await _commandRepository.UpdateAsync(entity, cancellationToken);

            _logger.LogInformation("Notification setting with id {Id} updated", dto.Id);

            return entity.ToNotificationSettingDto();
        }

        public async Task<NotificationSettingDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _queryRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Notification setting with id {Id} not found", id);
                return null;
            }
            return entity.ToNotificationSettingDto();
        }
    }
}
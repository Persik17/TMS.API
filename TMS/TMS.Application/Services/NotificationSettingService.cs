using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.DTOs.NotificationSetting;
using TMS.Application.Extensions;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSettingService"/> class.
        /// </summary>
        /// <param name="commandRepository">The repository for performing notification setting commands (e.g., insert, update).</param>
        /// <param name="queryRepository">The repository for performing notification setting queries (e.g., get by id).</param>
        /// <param name="logger">The logger for logging notification setting service events.</param>
        public NotificationSettingService(
            ICommandRepository<NotificationSetting> commandRepository,
            IQueryRepository<NotificationSetting> queryRepository,
            ILogger<NotificationSettingService> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<NotificationSettingDto> CreateAsync(NotificationSettingCreateDto createDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new notification setting");

            var entity = createDto.ToNotificationSetting();
            await _commandRepository.InsertAsync(entity, cancellationToken);

            _logger.LogInformation("Notification setting created with id: {Id}", entity.Id);

            return entity.ToNotificationSettingDto();
        }

        /// <inheritdoc/>

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

        /// <inheritdoc/>
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
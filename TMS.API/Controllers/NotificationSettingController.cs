using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.API.ViewModels.NotificationSetting;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.NotificationSetting;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for managing notification settings.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationSettingController : ControllerBase
    {
        private readonly INotificationSettingService _service;
        private readonly ILogger<NotificationSettingController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSettingController"/> class.
        /// </summary>
        /// <param name="service">The notification setting service.</param>
        /// <param name="logger">The logger.</param>
        public NotificationSettingController(
            INotificationSettingService service,
            ILogger<NotificationSettingController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a notification setting by its ID.
        /// </summary>
        /// <param name="id">The ID of the notification setting to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The notification setting view model.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(NotificationSettingViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NotificationSettingViewModel>> GetById(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var dto = await _service.GetByIdAsync(id, userId, cancellationToken);
            if (dto == null)
            {
                _logger.LogWarning("Notification setting with id {Id} not found", id);
                return NotFound();
            }

            return Ok(new NotificationSettingViewModel
            {
                Id = dto.Id,
                EmailNotificationsEnabled = dto.EmailNotificationsEnabled,
                PushNotificationsEnabled = dto.PushNotificationsEnabled,
                TelegramNotificationsEnabled = dto.TelegramNotificationsEnabled
            });
        }

        /// <summary>
        /// Creates a new notification setting.
        /// </summary>
        /// <param name="request">The request containing the notification setting data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created notification setting view model.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(NotificationSettingViewModel), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NotificationSettingViewModel>> Create([FromBody] NotificationSettingCreateViewModel request, Guid userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Create called with null model");
                return BadRequest("Data is required.");
            }

            var dto = new NotificationSettingCreateDto
            {
                EmailNotificationsEnabled = request.EmailNotificationsEnabled,
                PushNotificationsEnabled = request.PushNotificationsEnabled,
                TelegramNotificationsEnabled = request.TelegramNotificationsEnabled
            };

            var created = await _service.CreateAsync(dto, userId, cancellationToken);

            return Ok(new NotificationSettingViewModel
            {
                Id = created.Id,
                EmailNotificationsEnabled = created.EmailNotificationsEnabled,
                PushNotificationsEnabled = created.PushNotificationsEnabled,
                TelegramNotificationsEnabled = created.TelegramNotificationsEnabled
            });
        }

        /// <summary>
        /// Updates an existing notification setting.
        /// </summary>
        /// <param name="id">The ID of the notification setting to update.</param>
        /// <param name="request">The request containing the updated notification setting data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated notification setting view model.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(NotificationSettingViewModel), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NotificationSettingViewModel>> Update(Guid id, [FromBody] NotificationSettingViewModel request, Guid userId, CancellationToken cancellationToken)
        {
            if (request == null || id != request.Id)
            {
                _logger.LogWarning("Update called with invalid model or ID mismatch. Id: {Id}", id);
                return BadRequest("Invalid data.");
            }

            var dto = new NotificationSettingDto
            {
                Id = request.Id,
                EmailNotificationsEnabled = request.EmailNotificationsEnabled,
                PushNotificationsEnabled = request.PushNotificationsEnabled,
                TelegramNotificationsEnabled = request.TelegramNotificationsEnabled
            };

            var updated = await _service.UpdateAsync(dto, userId, cancellationToken);

            return Ok(new NotificationSettingViewModel
            {
                Id = updated.Id,
                EmailNotificationsEnabled = updated.EmailNotificationsEnabled,
                PushNotificationsEnabled = updated.PushNotificationsEnabled,
                TelegramNotificationsEnabled = updated.TelegramNotificationsEnabled
            });
        }
    }
}
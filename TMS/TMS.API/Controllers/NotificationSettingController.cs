using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.API.ViewModels.NotificationSetting;
using TMS.Application.Models.DTOs.NotificationSetting;

namespace TMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationSettingController : ControllerBase
    {
        private readonly INotificationSettingService<NotificationSettingDto, NotificationSettingCreateDto> _service;
        private readonly ILogger<NotificationSettingController> _logger;

        public NotificationSettingController(
            INotificationSettingService<NotificationSettingDto, NotificationSettingCreateDto> service,
            ILogger<NotificationSettingController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<NotificationSettingViewModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.GetByIdAsync(id, cancellationToken);
            if (dto == null)
                return NotFound();

            return Ok(new NotificationSettingViewModel
            {
                Id = dto.Id,
                EmailNotificationsEnabled = dto.EmailNotificationsEnabled,
                PushNotificationsEnabled = dto.PushNotificationsEnabled,
                TelegramNotificationsEnabled = dto.TelegramNotificationsEnabled
            });
        }

        [HttpPost]
        public async Task<ActionResult<NotificationSettingViewModel>> Create([FromBody] NotificationSettingCreateViewModel model, CancellationToken cancellationToken)
        {
            if (model == null)
                return BadRequest("Data is required.");

            var dto = new NotificationSettingCreateDto
            {
                EmailNotificationsEnabled = model.EmailNotificationsEnabled,
                PushNotificationsEnabled = model.PushNotificationsEnabled,
                TelegramNotificationsEnabled = model.TelegramNotificationsEnabled
            };

            var created = await _service.CreateAsync(dto, cancellationToken);

            return Ok(new NotificationSettingViewModel
            {
                Id = created.Id,
                EmailNotificationsEnabled = created.EmailNotificationsEnabled,
                PushNotificationsEnabled = created.PushNotificationsEnabled,
                TelegramNotificationsEnabled = created.TelegramNotificationsEnabled
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<NotificationSettingViewModel>> Update(Guid id, [FromBody] NotificationSettingViewModel model, CancellationToken cancellationToken)
        {
            if (model == null || id != model.Id)
                return BadRequest("Invalid data.");

            var dto = new NotificationSettingDto
            {
                Id = model.Id,
                EmailNotificationsEnabled = model.EmailNotificationsEnabled,
                PushNotificationsEnabled = model.PushNotificationsEnabled,
                TelegramNotificationsEnabled = model.TelegramNotificationsEnabled
            };

            var updated = await _service.UpdateAsync(dto, cancellationToken);

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
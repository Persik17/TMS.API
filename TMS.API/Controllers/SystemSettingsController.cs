using Microsoft.AspNetCore.Mvc;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.User;

namespace TMS.API.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/system-settings")]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingsService _service;

        public SystemSettingsController(ISystemSettingsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<SystemSettingsDto>> Get(Guid userId, CancellationToken cancellationToken)
        {
            var settings = await _service.GetByUserIdAsync(userId, cancellationToken);
            if (settings == null)
                return NotFound();
            return Ok(settings);
        }

        [HttpPost]
        public async Task<ActionResult<SystemSettingsDto>> Create(Guid userId, [FromBody] SystemSettingsDto createDto, CancellationToken cancellationToken)
        {
            if (createDto.UserId != userId)
                return BadRequest("UserId mismatch");
            var result = await _service.CreateAsync(createDto, userId, cancellationToken);
            return CreatedAtAction(nameof(Get), new { userId = result.UserId }, result);
        }

        [HttpPut]
        public async Task<ActionResult<SystemSettingsDto>> Update(Guid userId, [FromBody] SystemSettingsDto updateDto, CancellationToken cancellationToken)
        {
            if (updateDto.UserId != userId)
                return BadRequest("UserId mismatch");
            var result = await _service.UpdateAsync(updateDto, userId, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid userId, Guid id, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(id, userId, cancellationToken);
            return NoContent();
        }
    }
}

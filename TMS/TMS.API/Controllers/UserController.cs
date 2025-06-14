using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.Abstractions.Models.DTOs;
using TMS.API.ViewModels.TelegramAccount;
using TMS.Application.Models.DTOs.User;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for managing users (CRUD).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService<UserDto, UserCreateDto> _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService<UserDto, UserCreateDto> userService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User with id {Id} not found", id);
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserCreateDto createDto, CancellationToken cancellationToken)
        {
            if (createDto == null)
            {
                _logger.LogWarning("Create called with null DTO");
                return BadRequest("User data is required.");
            }

            var user = await _userService.CreateAsync(createDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDto updateDto, CancellationToken cancellationToken)
        {
            if (updateDto == null)
            {
                _logger.LogWarning("Update called with null DTO");
                return BadRequest("User data is required.");
            }
            if (id != updateDto.Id)
            {
                _logger.LogWarning("Update id mismatch: route id {RouteId}, body id {BodyId}", id, updateDto.Id);
                return BadRequest("ID mismatch");
            }

            await _userService.UpdateAsync(updateDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("{userId:guid}/telegram-account/link")]
        public async Task<IActionResult> LinkTelegramAccount(Guid userId, [FromBody] TelegramAccountCreateViewModel model, CancellationToken cancellationToken)
        {
            if (model == null)
                return BadRequest("Data is required.");

            var dto = new TelegramAccountCreateDto
            {
                NickName = model.NickName,
                Phone = model.Phone
            };

            await _userService.LinkTelegramAccountAsync(userId, dto, cancellationToken);
            return NoContent();
        }

        [HttpPost("{userId:guid}/telegram-account/unlink")]
        public async Task<IActionResult> UnlinkTelegramAccount(Guid userId, CancellationToken cancellationToken)
        {
            await _userService.UnlinkTelegramAccountAsync(userId, cancellationToken);
            return NoContent();
        }
    }
}
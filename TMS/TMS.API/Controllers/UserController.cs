using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.Abstractions.Models.DTOs.User;
using TMS.Application.DTOs.User;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="logger">The logger.</param>
        public UserController(
            IUserService<UserDto, UserCreateDto> userService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The user DTO.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="request">The request containing the user data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created user DTO.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserCreateDto request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Create called with null DTO");
                return BadRequest("User data is required.");
            }

            var createDto = new UserCreateDto
            {
            };

            var user = await _userService.CreateAsync(createDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="request">The request containing the updated user data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDto request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Update called with null DTO");
                return BadRequest("User data is required.");
            }
            if (id != request.Id)
            {
                _logger.LogWarning("Update id mismatch: route id {RouteId}, body id {BodyId}", id, request.Id);
                return BadRequest("ID mismatch");
            }

            var updateDto = new UserDto
            {
                Id = request.Id
            };

            await _userService.UpdateAsync(updateDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting user with id {Id}", id);
            await _userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Links a Telegram account to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to link the account to.</param>
        /// <param name="request">The Telegram account data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpPost("{userId:guid}/telegram-account/link")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> LinkTelegramAccount(Guid userId, [FromBody] TelegramAccountCreateDto request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("LinkTelegramAccount called with null model");
                return BadRequest("Data is required.");
            }

            var dto = new TelegramAccountCreateDto
            {
                NickName = request.NickName,
                Phone = request.Phone
            };

            await _userService.LinkTelegramAccountAsync(userId, dto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Unlinks a Telegram account from a user.
        /// </summary>
        /// <param name="userId">The ID of the user to unlink the account from.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpPost("{userId:guid}/telegram-account/unlink")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UnlinkTelegramAccount(Guid userId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unlinking Telegram Account From User With Id {Id}", userId);
            await _userService.UnlinkTelegramAccountAsync(userId, cancellationToken);
            return NoContent();
        }
    }
}
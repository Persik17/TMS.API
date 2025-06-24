using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.Abstractions.Models.DTOs.Authentication;
using TMS.API.ViewModels.Authentication;
using TMS.API.ViewModels.Registration;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for handling user authentication.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IVerificationService _verificationService;
        private readonly ILogger<AuthenticationController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        /// <param name="logger">The logger.</param>
        public AuthenticationController(
            IAuthenticationService authService,
            IVerificationService verificationService,
            ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _verificationService = verificationService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="model">The authentication model containing the user's email and password.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the authentication attempt.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResultViewModel>> Login([FromBody] AuthenticationViewModel model, CancellationToken cancellationToken)
        {
            if (model == null)
            {
                _logger.LogWarning("Login called with null model");
                return BadRequest("Login data is required.");
            }

            var dto = new AuthenticationDto
            {
                Email = model.Email,
                Password = model.Password
            };

            try
            {
                var result = await _authService.AuthenticateAsync(dto, cancellationToken);
                if (!result.Success)
                {
                    _logger.LogWarning("Authentication failed for email {Email}", model.Email);
                    return Unauthorized(new AuthenticationResultViewModel
                    {
                        Success = false,
                        Error = result.Error ?? "Authentication failed."
                    });
                }
                _logger.LogInformation("User {Email} logged in successfully.", model.Email);
                return Ok(new AuthenticationResultViewModel
                {
                    Success = true,
                    Token = result.Token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during authentication for email {Email}", model.Email);
                return StatusCode(500, new AuthenticationResultViewModel
                {
                    Success = false,
                    Error = "An unexpected error occurred."
                });
            }
        }

        /// <summary>
        /// Confirms a user's registration with a verification code.
        /// </summary>
        /// <param name="request">The confirmation request data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The confirmation result.</returns>
        [HttpPost("confirm")]
        [ProducesResponseType(typeof(ConfirmationResultViewModel), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ConfirmationResultViewModel>> Confirm([FromBody] AuthenticationConfirmationViewModel request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Confirm called with null model");
                return BadRequest("Confirmation data is required.");
            }

            var dto = new AuthenticationConfirmationDto
            {
                VerificationId = request.VerificationId,
                Code = request.Code
            };

            var result = await _verificationService.ConfirmLoginAsync(dto, cancellationToken);

            return Ok(new ConfirmationResultViewModel
            {
                Success = result.Success,
                Error = result.Error
            });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Exceptions;
using TMS.API.ViewModels.Authentication;
using TMS.API.ViewModels.Verification;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Authentication;
using TMS.Application.Dto.Verification;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for handling user authentication and verification.
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
        /// <param name="verificationService">The verification service.</param>
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
        /// Initiates the authentication process for a user via email and password.
        /// Issues a verification code if credentials are valid.
        /// </summary>
        /// <param name="model">The authentication model containing the user's email and password.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Authentication result including verification details.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthenticationResultViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AuthenticationResultViewModel>> Login(
            [FromBody] AuthenticationViewModel model,
            CancellationToken cancellationToken)
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

                _logger.LogInformation("Verification code sent to user {Email}.", model.Email);
                return Ok(new AuthenticationResultViewModel
                {
                    Success = true,
                    VerificationId = result.VerificationId,
                    Expiration = result.Expiration
                });
            }
            catch (AuthenticationFailedException ex)
            {
                _logger.LogWarning(ex, "Authentication failed for email {Email}", model.Email);
                return Unauthorized(new AuthenticationResultViewModel
                {
                    Success = false,
                    Error = ex.Message
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
        /// Authenticates or registers a user via Telegram.
        /// </summary>
        /// <param name="model">The Telegram authentication model.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Authentication result with issued token.</returns>
        [HttpPost("telegram")]
        [ProducesResponseType(typeof(AuthenticationResultViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AuthenticationResultViewModel>> Telegram(
            [FromBody] TelegramAuthenticationViewModel model,
            CancellationToken cancellationToken)
        {
            if (model == null)
            {
                _logger.LogWarning("Telegram login called with null model");
                return BadRequest("Telegram authentication data is required.");
            }

            var dto = new TelegramAuthenticationDto
            {
                TelegramUserId = model.TelegramUserId,
                Username = model.Username,
                FullName = model.FullName,
                Phone = model.Phone,
                AuthDate = model.AuthDate,
                Hash = model.Hash
            };

            try
            {
                var result = await _authService.AuthenticateTelegramAsync(dto, cancellationToken);
                if (!result.Success)
                {
                    _logger.LogWarning("Telegram authentication failed for user {TelegramUserId}", model.TelegramUserId);
                    return Unauthorized(new AuthenticationResultViewModel
                    {
                        Success = false,
                        Error = result.Error ?? "Telegram authentication failed."
                    });
                }

                return Ok(new AuthenticationResultViewModel
                {
                    Success = true,
                    Token = result.Token,
                    UserId = result.UserId,
                    TelegramId = result.TelegramId,
                    FullName = result.FullName,
                    Email = result.Email
                });
            }
            catch (AuthenticationFailedException ex)
            {
                _logger.LogWarning(ex, "Telegram authentication failed for user {TelegramUserId}", model.TelegramUserId);
                return Unauthorized(new AuthenticationResultViewModel
                {
                    Success = false,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during Telegram authentication for user {TelegramUserId}", model.TelegramUserId);
                return StatusCode(500, new AuthenticationResultViewModel
                {
                    Success = false,
                    Error = "An unexpected error occurred."
                });
            }
        }

        /// <summary>
        /// Confirms a user's authentication or registration using a verification code.
        /// </summary>
        /// <param name="request">The confirmation request data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The confirmation result with authentication token if successful.</returns>
        [HttpPost("confirm")]
        [ProducesResponseType(typeof(ConfirmationResultViewModel), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ConfirmationResultViewModel>> Confirm(
            [FromBody] AuthenticationConfirmationViewModel request,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Confirm called with null model");
                return BadRequest("Confirmation data is required.");
            }

            var dto = new ConfirmationDto
            {
                VerificationId = request.VerificationId,
                Code = request.Code
            };

            var result = await _verificationService.ConfirmLoginAsync(dto, cancellationToken);

            return Ok(new ConfirmationResultViewModel
            {
                Success = true,
                Token = result.Token,
                UserId = result.UserId,
                CompanyId = result.CompanyId
            });
        }
    }
}
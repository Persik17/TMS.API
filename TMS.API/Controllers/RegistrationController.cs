using Microsoft.AspNetCore.Mvc;
using TMS.API.ViewModels.Registration;
using TMS.API.ViewModels.Verification;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Registration;
using TMS.Application.Dto.Verification;

namespace TMS.API.Controllers
{
    // <summary>
    /// Controller for user registration and verification.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly IVerificationService _verificationService;
        private readonly ILogger<RegistrationController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationController"/> class.
        /// </summary>
        public RegistrationController(
            IRegistrationService registrationService,
            IVerificationService verificationService,
            ILogger<RegistrationController> logger)
        {
            _registrationService = registrationService;
            _verificationService = verificationService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user and initiates verification.
        /// </summary>
        /// <param name="model">The registration model containing email and password.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Registration result with verification info.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegistrationResultViewModel), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RegistrationResultViewModel>> Register(
            [FromBody] RegistrationViewModel model,
            CancellationToken cancellationToken)
        {
            if (model == null)
            {
                _logger.LogWarning("Register called with null model");
                return BadRequest("Registration data is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                _logger.LogWarning("Register called with invalid Email");
                return BadRequest("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                _logger.LogWarning("Register called with empty password for email {Email}", model.Email);
                return BadRequest("Password is required.");
            }

            var dto = new RegistrationDto
            {
                Email = model.Email,
                Password = model.Password
            };

            try
            {
                var result = await _registrationService.RegisterAsync(dto, cancellationToken);

                if (!result.Success)
                {
                    _logger.LogWarning("Registration failed for {Email}: {Error}", model.Email, result.Error);
                    return BadRequest(new RegistrationResultViewModel
                    {
                        Success = false,
                        Error = result.Error
                    });
                }

                _logger.LogInformation("Registration verification sent to {Email}", model.Email);
                return Ok(new RegistrationResultViewModel
                {
                    Success = true,
                    VerificationId = result.VerificationId,
                    Expiration = result.Expiration
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during registration for {Email}", model.Email);
                return StatusCode(500, new RegistrationResultViewModel
                {
                    Success = false,
                    Error = "An unexpected error occurred."
                });
            }
        }

        /// <summary>
        /// Confirms a user's registration with a verification code.
        /// </summary>
        /// <param name="model">The confirmation request data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The confirmation result with auth token if successful.</returns>
        [HttpPost("confirm")]
        [ProducesResponseType(typeof(ConfirmationResultViewModel), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ConfirmationResultViewModel>> Confirm(
            [FromBody] RegistrationConfirmationViewModel model,
            CancellationToken cancellationToken)
        {
            if (model == null)
            {
                _logger.LogWarning("Confirm called with null model");
                return BadRequest("Confirmation data is required.");
            }

            if (model.VerificationId == Guid.Empty || string.IsNullOrWhiteSpace(model.Code))
            {
                _logger.LogWarning("Confirm called with invalid data (VerificationId or Code)");
                return BadRequest("VerificationId and Code are required.");
            }

            try
            {
                var dto = new ConfirmationDto
                {
                    VerificationId = model.VerificationId,
                    Code = model.Code
                };

                var result = await _verificationService.ConfirmRegistrationAsync(dto, cancellationToken);

                if (!result.Success)
                {
                    _logger.LogWarning("Registration confirmation failed for verificationId {VerificationId}: {Error}", model.VerificationId, result.Error);
                    return BadRequest(new ConfirmationResultViewModel
                    {
                        Success = false,
                        Error = result.Error
                    });
                }

                _logger.LogInformation("Registration confirmed for verificationId {VerificationId}", model.VerificationId);
                return Ok(new ConfirmationResultViewModel
                {
                    Success = true,
                    Token = result.Token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during registration confirmation for verificationId {VerificationId}", model.VerificationId);
                return StatusCode(500, new ConfirmationResultViewModel
                {
                    Success = false,
                    Error = "An unexpected error occurred."
                });
            }
        }
    }
}
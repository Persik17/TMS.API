using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.Abstractions.Models.DTOs.Registration;
using TMS.API.ViewModels.Registration;
using TMS.Contracts.Enums;

namespace TMS.API.Controllers
{
    /// <summary>
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
        /// <param name="registrationService">The registration service.</param>
        /// <param name="verificationService">The verification service.</param>
        /// <param name="logger">The logger.</param>
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
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration request data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The registration result.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegistrationResultViewModel), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RegistrationResultViewModel>> Register([FromBody] RegistrationViewModel request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Register called with null model");
                return BadRequest("Registration data is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Target))
            {
                _logger.LogWarning("Register called with invalid Target");
                return BadRequest("Target is required.");
            }

            var dto = new RegistrationDto
            {
                Target = request.Target,
                Type = (NotificationType)request.Type,
                Password = request.Password
            };

            var result = await _registrationService.RegisterAsync(dto, cancellationToken);

            return Ok(new RegistrationResultViewModel
            {
                Success = result.Success,
                VerificationId = result.VerificationId,
                Expiration = result.Expiration,
                Error = result.Error
            });
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
        public async Task<ActionResult<ConfirmationResultViewModel>> Confirm([FromBody] RegistrationConfirmationViewModel request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Confirm called with null model");
                return BadRequest("Confirmation data is required.");
            }

            var dto = new RegistrationConfirmationDto
            {
                VerificationId = request.VerificationId,
                Code = request.Code
            };

            var result = await _verificationService.ConfirmRegistrationAsync(dto, cancellationToken);

            return Ok(new ConfirmationResultViewModel
            {
                Success = result.Success,
                Error = result.Error
            });
        }
    }
}
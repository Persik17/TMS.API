using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.API.ViewModels.Registration;
using TMS.Application.Models.DTOs.Registration;
using TMS.Contracts.Enums;

namespace TMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService<RegistrationDto, RegistrationResultDto> _registrationService;
        private readonly IVerificationService<RegistrationConfirmationDto, ConfirmationResultDto> _verificationService;
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(
            IRegistrationService<RegistrationDto, RegistrationResultDto> registrationService,
            IVerificationService<RegistrationConfirmationDto, ConfirmationResultDto> verificationService,
            ILogger<RegistrationController> logger)
        {
            _registrationService = registrationService;
            _verificationService = verificationService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<RegistrationResultViewModel>> Register([FromBody] RegistrationViewModel model, CancellationToken cancellationToken)
        {
            if (model == null)
                return BadRequest("Registration data is required.");

            var dto = new RegistrationDto
            {
                Target = model.Target,
                Type = (VerificationType)model.Type,
                Password = model.Password
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

        [HttpPost("confirm")]
        public async Task<ActionResult<ConfirmationResultViewModel>> Confirm([FromBody] RegistrationConfirmationViewModel model, CancellationToken cancellationToken)
        {
            if (model == null)
                return BadRequest("Confirmation data is required.");

            var dto = new RegistrationConfirmationDto
            {
                VerificationId = model.VerificationId,
                Code = model.Code
            };

            var result = await _verificationService.ConfirmAsync(dto, cancellationToken);

            return Ok(new ConfirmationResultViewModel
            {
                Success = result.Success,
                Error = result.Error
            });
        }
    }
}
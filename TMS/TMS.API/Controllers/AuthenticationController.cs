using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.API.ViewModels.Authentication;
using TMS.Application.Models.DTOs.Authentication;

namespace TMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService<AuthenticationDto, AuthenticationResultDto> _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            IAuthenticationService<AuthenticationDto, AuthenticationResultDto> authService,
            ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

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
                    return Unauthorized(new AuthenticationResultViewModel
                    {
                        Success = false,
                        Error = result.Error ?? "Authentication failed."
                    });
                }
                return Ok(new AuthenticationResultViewModel
                {
                    Success = true,
                    Token = result.Token
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Authentication failed for email {Email}", model.Email);
                return Unauthorized(new AuthenticationResultViewModel
                {
                    Success = false,
                    Error = "Authentication failed."
                });
            }
        }
    }
}
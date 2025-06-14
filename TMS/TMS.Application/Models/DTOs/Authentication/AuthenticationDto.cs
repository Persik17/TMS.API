using TMS.Abstractions.Models.Interfaces;

namespace TMS.Application.Models.DTOs.Authentication
{
    public class AuthenticationDto : IAuthenticateDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
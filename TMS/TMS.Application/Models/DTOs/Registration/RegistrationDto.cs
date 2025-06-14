using TMS.Abstractions.Models.Interfaces;
using TMS.Contracts.Enums;

namespace TMS.Application.Models.DTOs.Registration
{
    public class RegistrationDto : IRegisterDto
    {
        public string Target { get; set; }
        public string Password { get; set; }
        public VerificationType Type { get; set; }
    }
}
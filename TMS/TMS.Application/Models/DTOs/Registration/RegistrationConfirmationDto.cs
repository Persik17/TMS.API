using TMS.Abstractions.Models.Interfaces;

namespace TMS.Application.Models.DTOs.Registration
{
    public class RegistrationConfirmationDto : IConfirmDto
    {
        public Guid VerificationId { get; set; }
        public string Code { get; set; }
    }
}

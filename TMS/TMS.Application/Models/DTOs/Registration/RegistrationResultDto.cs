namespace TMS.Application.Models.DTOs.Registration
{
    public class RegistrationResultDto
    {
        public Guid VerificationId { get; set; }
        public DateTime Expiration { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}

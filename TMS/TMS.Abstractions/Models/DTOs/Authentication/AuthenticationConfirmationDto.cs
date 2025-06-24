namespace TMS.Abstractions.Models.DTOs.Authentication
{
    /// <summary>
    /// Represents the data transfer object (DTO) for confirming a registration via a verification code.
    /// </summary>
    public class AuthenticationConfirmationDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the verification record.
        /// </summary>
        public Guid VerificationId { get; set; }

        /// <summary>
        /// Gets or sets the verification code required to confirm the registration.
        /// </summary>
        public string Code { get; set; }
    }
}

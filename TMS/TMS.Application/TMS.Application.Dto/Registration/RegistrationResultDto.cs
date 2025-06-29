namespace TMS.Application.Dto.Registration
{
    /// <summary>
    /// Represents the data transfer object (DTO) for the result of a user registration attempt.
    /// </summary>
    public class RegistrationResultDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the verification record created during registration.
        /// </summary>
        public Guid VerificationId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the verification record expires.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the registration was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error message if the registration failed. This property will be <c>null</c> if the registration was successful.
        /// </summary>
        public string Error { get; set; }
    }
}

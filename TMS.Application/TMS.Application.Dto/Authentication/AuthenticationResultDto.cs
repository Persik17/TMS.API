namespace TMS.Application.Dto.Authentication
{
    /// <summary>
    /// Represents the data transfer object (DTO) for the result of an authentication attempt.
    /// </summary>
    public class AuthenticationResultDto
    {
        /// <summary>
        /// Gets or sets the verification identifier (used for email/SMS verification flows).
        /// </summary>
        public Guid? VerificationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the authentication was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the authentication token if the authentication was successful.
        /// This property will be <c>null</c> if authentication failed.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Gets or sets the error message if the authentication failed.
        /// This property will be <c>null</c> if authentication was successful.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the user identifier (if authentication was successful).
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the Telegram account identifier (if authenticated/registered via Telegram).
        /// </summary>
        public Guid? TelegramId { get; set; }

        /// <summary>
        /// Gets or sets the user's display name (if available).
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Gets or sets the user's email (if available).
        /// </summary>
        public string? Email { get; set; }
        public DateTime Expiration { get; set; }
    }
}

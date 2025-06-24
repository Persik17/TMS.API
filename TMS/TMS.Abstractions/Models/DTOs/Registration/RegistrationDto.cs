using TMS.Contracts.Enums;

namespace TMS.Abstractions.Models.DTOs.Registration
{
    /// <summary>
    /// Represents the data transfer object (DTO) for user registration.
    /// </summary>
    public class RegistrationDto
    {
        /// <summary>
        /// Gets or sets the target for registration (e.g., email address or phone number).
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the user's desired password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the notification sending type (e.g., email, SMS).
        /// </summary>
        public NotificationType Type { get; set; }
    }
}
namespace TMS.Abstractions.Models.DTOs.User
{
    /// <summary>
    /// Represents the data transfer object (DTO) for creating a user.
    /// </summary>
    public class UserCreateDto
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Telegram identifier of the user, if available.
        /// </summary>
        public Guid? TelegramId { get; set; }

        /// <summary>
        /// Gets or sets the time zone of the user (e.g., "Europe/Moscow").
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// Gets or sets the preferred language of the user (e.g., "en", "ru").
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the status of the user.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the user's notification settings.
        /// </summary>
        public Guid NotificationSettingsId { get; set; }
    }
}

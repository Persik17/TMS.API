using System.ComponentModel.DataAnnotations;

namespace TMS.Application.Dto.Authentication
{
    /// <summary>
    /// Data Transfer Object for Telegram authentication.
    /// </summary>
    public class TelegramAuthenticationDto
    {
        /// <summary>
        /// Telegram user ID (from Telegram).
        /// </summary>
        [Required]
        public long TelegramUserId { get; set; }

        /// <summary>
        /// Telegram username (nickname).
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// User's phone number (if provided).
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// User's display name.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// User's chat ID (if available).
        /// </summary>
        public long? ChatId { get; set; }

        /// <summary>
        /// Unix timestamp of authentication event.
        /// </summary>
        public long AuthDate { get; set; }

        /// <summary>
        /// The Telegram-provided hash (for signature verification).
        /// </summary>
        [Required]
        public string Hash { get; set; }
    }
}

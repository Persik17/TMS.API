namespace TMS.API.ViewModels.Authentication
{
    /// <summary>
    /// Result model for authentication requests.
    /// </summary>
    public class AuthenticationResultViewModel
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public Guid? VerificationId { get; set; }
        public DateTime? Expiration { get; set; }
        public string Error { get; set; }

        // Telegram-specific (optional, для Telegram ответа)
        public Guid? UserId { get; set; }
        public Guid? TelegramId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
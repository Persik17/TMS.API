namespace TMS.API.ViewModels.Authentication
{
    /// <summary>
    /// Model for Telegram authentication.
    /// </summary>
    public class TelegramAuthenticationViewModel
    {
        public long TelegramUserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public long AuthDate { get; set; }
        public string Hash { get; set; }
    }
}

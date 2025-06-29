namespace TMS.Application.Dto.TelegramAccount
{
    /// <summary>
    /// Represents the data transfer object (DTO) for creating a new Telegram account.
    /// </summary>
    public class TelegramAccountCreateDto
    {
        /// <summary>
        /// Gets or sets the Telegram account's nickname.
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets the Telegram account's phone number.
        /// </summary>
        public string Phone { get; set; }
    }
}
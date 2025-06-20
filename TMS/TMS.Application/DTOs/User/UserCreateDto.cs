namespace TMS.Application.DTOs.User
{
    public class UserCreateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid? TelegramId { get; set; }
        public string Timezone { get; set; }
        public string Language { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public Guid NotificationSettingsId { get; set; }
    }
}

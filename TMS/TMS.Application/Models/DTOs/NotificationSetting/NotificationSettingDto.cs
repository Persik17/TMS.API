namespace TMS.Application.Models.DTOs.NotificationSetting
{
    public class NotificationSettingDto
    {
        public Guid Id { get; set; }
        public bool EmailNotificationsEnabled { get; set; }
        public bool PushNotificationsEnabled { get; set; }
        public bool TelegramNotificationsEnabled { get; set; }
    }
}
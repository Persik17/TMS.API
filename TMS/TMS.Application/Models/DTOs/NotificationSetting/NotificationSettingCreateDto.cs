namespace TMS.Application.Models.DTOs.NotificationSetting
{
    public class NotificationSettingCreateDto
    {
        public bool EmailNotificationsEnabled { get; set; }
        public bool PushNotificationsEnabled { get; set; }
        public bool TelegramNotificationsEnabled { get; set; }
    }
}
namespace TMS.API.ViewModels.NotificationSetting
{
    public class NotificationSettingCreateViewModel
    {
        public bool EmailNotificationsEnabled { get; set; }
        public bool PushNotificationsEnabled { get; set; }
        public bool TelegramNotificationsEnabled { get; set; }
    }
}
namespace TMS.API.ViewModels.NotificationSetting
{
    public class NotificationSettingViewModel
    {
        public Guid Id { get; set; }
        public bool EmailNotificationsEnabled { get; set; }
        public bool PushNotificationsEnabled { get; set; }
        public bool TelegramNotificationsEnabled { get; set; }
    }
}
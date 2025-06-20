using TMS.Application.DTOs.NotificationSetting;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Extensions
{
    public static class NotificationSettingMappingExtensions
    {
        public static NotificationSetting ToNotificationSetting(this NotificationSettingCreateDto dto)
        {
            return new NotificationSetting
            {
                EmailNotificationsEnabled = dto.EmailNotificationsEnabled,
                PushNotificationsEnabled = dto.PushNotificationsEnabled,
                TelegramNotificationsEnabled = dto.TelegramNotificationsEnabled
            };
        }

        public static NotificationSettingDto ToNotificationSettingDto(this NotificationSetting entity)
        {
            return new NotificationSettingDto
            {
                Id = entity.Id,
                EmailNotificationsEnabled = entity.EmailNotificationsEnabled,
                PushNotificationsEnabled = entity.PushNotificationsEnabled,
                TelegramNotificationsEnabled = entity.TelegramNotificationsEnabled
            };
        }
    }
}
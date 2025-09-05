using TMS.Abstractions.Enums;
using TMS.Application.Dto.User;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Extensions
{
    public static class UserMappingExtensions
    {
        public static User ToUser(this UserCreateDto dto)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Email = dto.Email,
                TelegramId = dto.TelegramId,
                Timezone = dto.Timezone,
                Language = dto.Language,
                Phone = dto.Phone,
                Status = dto.Status,
                NotificationSettingsId = dto.NotificationSettingsId,
                CreationDate = DateTime.UtcNow
            };
        }

        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = string.IsNullOrWhiteSpace(user.FullName) ? user.Email : user.FullName,
                Email = user.Email,
                TelegramId = user.TelegramId,
                Timezone = user.Timezone,
                Language = user.Language,
                Phone = user.Phone,
                Status = user.Status,
                NotificationSettingsId = user.NotificationSettingsId,
                NotificationSettings = user.NotificationSettings?.ToNotificationSettingDto(),
                SystemSettingsId = user.SystemSettingsId,
                SystemSettings = user.SystemSettings?.ToSystemSettingsDto(),
                LastLoginDate = user.LastLoginDate,
                CreationDate = user.CreationDate,
                UpdateDate = user.UpdateDate,
                DeleteDate = user.DeleteDate
            };
        }

        public static User ToUser(this UserInviteDto dto)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Email = dto.Email,
                Language = dto.Language,
                Status = (int)UserStatus.Invited,
                CreationDate = DateTime.UtcNow
            };
        }
    }
}
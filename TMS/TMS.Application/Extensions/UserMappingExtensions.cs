using TMS.Application.Models.DTOs.User;
using TMS.Infrastructure.DataAccess.DataModels;

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
                RegistrationDate = DateTime.UtcNow,
                CreationDate = DateTime.UtcNow
            };
        }

        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                TelegramId = user.TelegramId,
                Timezone = user.Timezone,
                Language = user.Language,
                Phone = user.Phone,
                Status = user.Status,
                NotificationSettingsId = user.NotificationSettingsId,
                RegistrationDate = user.RegistrationDate,
                LastLoginDate = user.LastLoginDate,
                CreationDate = user.CreationDate,
                UpdateDate = user.UpdateDate,
                DeleteDate = user.DeleteDate
            };
        }
    }
}
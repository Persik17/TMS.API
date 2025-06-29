using TMS.Abstractions.Enums;
using TMS.Application.Abstractions.Factories;
using TMS.Application.Dto.Authentication;
using TMS.Application.Dto.User;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Factories
{
    /// <summary>
    /// Provides methods for creating base user DTOs with various initial parameters.
    /// </summary>
    public class UserFactory : IUserFactory
    {
        public User CreatePendingUserByEmail(string email)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Email = email,
                Status = (int)UserStatus.Pending,
            };
        }

        public User CreateUserFromTelegram(TelegramAuthenticationDto dto, Guid telegramAccountId)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow,
                FullName = dto.FullName,
                TelegramId = telegramAccountId,
                Phone = dto.Phone,
                Status = (int)UserStatus.Active,
            };
        }

        public User CreateCustomUser(UserCreateDto dto)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Email = dto.Email,
                FullName = dto.FullName,
                Phone = dto.Phone,
                Timezone = dto.Timezone,
                Language = dto.Language,
                Status = dto.Status,
            };
        }
    }
}

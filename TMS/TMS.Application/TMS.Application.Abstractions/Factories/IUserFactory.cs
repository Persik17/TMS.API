using TMS.Application.Dto.Authentication;
using TMS.Application.Dto.User;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Abstractions.Factories
{
    public interface IUserFactory
    {
        User CreatePendingUserByEmail(string email);

        User CreateUserFromTelegram(TelegramAuthenticationDto dto, Guid telegramAccountId);

        User CreateCustomUser(UserCreateDto dto);
    }
}

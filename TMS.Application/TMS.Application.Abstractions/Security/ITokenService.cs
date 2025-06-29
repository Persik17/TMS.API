using TMS.Application.Dto.User;

namespace TMS.Application.Abstractions.Security
{
    public interface ITokenService
    {
        string GenerateToken(UserDto user);
        bool ValidateToken(string token, out Guid userId);
    }
}

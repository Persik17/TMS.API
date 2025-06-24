using TMS.Abstractions.Interfaces.Factories;
using TMS.Abstractions.Models.DTOs.User;

namespace TMS.Application.Services.Factories
{
    /// <summary>
    /// Provides methods for creating base user DTOs with various initial parameters.
    /// </summary>
    public class UserFactory : IUserFactory
    {
        /// <summary>
        /// Creates a base user DTO with default language and status.
        /// </summary>
        /// <returns>A new <see cref="UserCreateDto"/> instance with default language ("en-US") and status (0).</returns>
        public UserCreateDto CreateBaseUser()
        {
            return new UserCreateDto
            {
                Language = "en-US",
                Status = 0
            };
        }

        /// <summary>
        /// Creates a base user DTO initialized with the specified email, default language, and status.
        /// </summary>
        /// <param name="email">The email address to assign to the new user.</param>
        /// <returns>A new <see cref="UserCreateDto"/> instance initialized with the given email, default language ("en-US"), and status (0).</returns>
        public UserCreateDto CreateBaseUserByEmail(string email)
        {
            return new UserCreateDto
            {
                Email = email,
                Language = "en-US",
                Status = 0
            };
        }

        /// <summary>
        /// Creates a base user DTO initialized with the specified phone as the email, default language, and status.
        /// </summary>
        /// <param name="phone">The phone number to assign to the new user (assigned to the Email property).</param>
        /// <returns>A new <see cref="UserCreateDto"/> instance initialized with the given phone as email, default language ("en-US"), and status (0).</returns>
        public UserCreateDto CreateBaseUserByPhone(string phone)
        {
            return new UserCreateDto
            {
                Email = phone,
                Language = "en-US",
                Status = 0
            };
        }
    }
}

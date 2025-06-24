using TMS.Abstractions.Models.DTOs.User;

namespace TMS.Abstractions.Interfaces.Factories
{
    /// <summary>
    /// Factory interface for creating base user DTOs with various initial parameters.
    /// </summary>
    public interface IUserFactory
    {
        /// <summary>
        /// Creates a base user DTO with default parameters.
        /// </summary>
        /// <returns>A new <see cref="UserCreateDto"/> instance with default values.</returns>
        UserCreateDto CreateBaseUser();

        /// <summary>
        /// Creates a base user DTO initialized with the specified email.
        /// </summary>
        /// <param name="email">The email address to assign to the new user.</param>
        /// <returns>A new <see cref="UserCreateDto"/> instance initialized with the given email.</returns>
        UserCreateDto CreateBaseUserByEmail(string email);

        /// <summary>
        /// Creates a base user DTO initialized with the specified phone number.
        /// </summary>
        /// <param name="phone">The phone number to assign to the new user.</param>
        /// <returns>A new <see cref="UserCreateDto"/> instance initialized with the given phone number.</returns>
        UserCreateDto CreateBaseUserByPhone(string phone);
    }
}

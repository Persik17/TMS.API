using TMS.Abstractions.Models.DTOs.Authentication;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for user authentication (login).
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates a user asynchronously and returns an authentication result.
        /// </summary>
        /// <param name="dto">The DTO containing login data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous authentication operation. The task result contains the authentication result (token, user info, success status, error messages, etc.).</returns>
        /// <remarks>
        /// The authentication process may succeed, fail due to invalid credentials, or fail due to other errors (e.g., database connection issues).
        /// The <see cref="AuthenticationResultDto"/> contains information about the success status and any potential errors.
        /// </remarks>
        Task<AuthenticationResultDto> AuthenticateAsync(AuthenticationDto dto, CancellationToken cancellationToken = default);
    }
}
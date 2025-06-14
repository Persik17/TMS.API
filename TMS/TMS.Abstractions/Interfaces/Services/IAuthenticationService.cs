using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for user authentication (login).
    /// </summary>
    /// <typeparam name="TAuthDto">DTO used for authentication request.</typeparam>
    /// <typeparam name="TAuthResultDto">DTO used for authentication result.</typeparam>
    public interface IAuthenticationService<TAuthDto, TAuthResultDto>
        where TAuthDto : class, IAuthenticateDto
        where TAuthResultDto : class
    {
        /// <summary>
        /// Authenticates a user and returns a token or session info.
        /// </summary>
        /// <param name="dto">Login data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Authentication result (token, user info, etc.).</returns>
        Task<TAuthResultDto> AuthenticateAsync(TAuthDto dto, CancellationToken cancellationToken = default);
    }
}
using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for user registration.
    /// </summary>
    /// <typeparam name="TRegisterDto">DTO used for registration request.</typeparam>
    /// <typeparam name="TRegisterResultDto">DTO used for registration result.</typeparam>
    public interface IRegistrationService<TRegisterDto, TRegisterResultDto>
        where TRegisterDto : class, IRegisterDto
        where TRegisterResultDto : class
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="dto">Registration data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result of registration (user id, status, etc.).</returns>
        Task<TRegisterResultDto> RegisterAsync(TRegisterDto dto, CancellationToken cancellationToken = default);
    }
}
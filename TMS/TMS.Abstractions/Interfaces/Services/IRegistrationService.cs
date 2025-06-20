using TMS.Abstractions.Models.DTOs.Registration;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for user registration.
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing registration data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous registration operation. The task result contains the registration result (user ID, status, etc.).</returns>
        /// <remarks>
        /// The registration process may succeed, fail due to invalid input, or fail due to other errors (e.g., database connection issues).
        /// The <see cref="RegistrationResultDto"/> contains information about the success status and any potential errors.
        /// </remarks>
        Task<RegistrationResultDto> RegisterAsync(RegistrationDto dto, CancellationToken cancellationToken = default);
    }
}
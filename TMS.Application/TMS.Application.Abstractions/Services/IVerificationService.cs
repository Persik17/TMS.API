using TMS.Application.Dto.Verification;

namespace TMS.Application.Abstractions.Services
{
    /// <summary>
    /// Service contract for verification of registration.
    /// </summary>
    public interface IVerificationService
    {
        /// <summary>
        /// Confirms registration by code asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing registration confirmation data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous confirmation operation. The task result contains the confirmation result.</returns>
        /// <remarks>
        /// The confirmation process may succeed, fail due to an invalid code, or fail due to other errors (e.g., code expired).
        /// The <see cref="ConfirmationResultDto"/> contains information about the success status and any potential errors.
        /// </remarks>
        Task<ConfirmationResultDto> ConfirmRegistrationAsync(ConfirmationDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Confirms login by code asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing authentication confirmation data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous confirmation operation. The task result contains the confirmation result.</returns>
        /// <remarks>
        /// The confirmation process may succeed, fail due to an invalid code, or fail due to other errors (e.g., code expired).
        /// The <see cref="ConfirmationResultDto"/> contains information about the success status and any potential errors.
        /// </remarks>
        Task<ConfirmationResultDto> ConfirmLoginAsync(ConfirmationDto dto, CancellationToken cancellationToken = default);
    }
}
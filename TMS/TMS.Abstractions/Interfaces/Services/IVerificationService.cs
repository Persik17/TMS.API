using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for verification of registration.
    /// </summary>
    /// <typeparam name="TConfirmDto">DTO used for confirmation request.</typeparam>
    /// <typeparam name="TConfirmResultDto">DTO used for confirmation result.</typeparam>
    public interface IVerificationService<TConfirmDto, TConfirmResultDto>
        where TConfirmDto : class, IConfirmDto
        where TConfirmResultDto : class
    {
        /// <summary>
        /// Confirms registration by code.
        /// </summary>
        Task<TConfirmResultDto> ConfirmAsync(TConfirmDto dto, CancellationToken cancellationToken = default);
    }
}
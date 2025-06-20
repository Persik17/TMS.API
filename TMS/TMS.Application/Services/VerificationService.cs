using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Interfaces.Services;
using TMS.Abstractions.Models.DTOs.Registration;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for confirming registration by code.
    /// </summary>
    public class VerificationService : IVerificationService
    {
        private readonly ICommandRepository<RegistrationVerification> _commandRepository;
        private readonly IQueryRepository<RegistrationVerification> _queryRepository;
        private readonly ILogger<VerificationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationService"/> class.
        /// </summary>
        /// <param name="commandRepository">The repository for performing registration verification commands (e.g., insert, update).</param>
        /// <param name="queryRepository">The repository for performing registration verification queries (e.g., get by id).</param>
        /// <param name="logger">The logger for logging verification service events.</param>
        public VerificationService(
            ICommandRepository<RegistrationVerification> commandRepository,
            IQueryRepository<RegistrationVerification> queryRepository,
            ILogger<VerificationService> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ConfirmationResultDto> ConfirmAsync(RegistrationConfirmationDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var verification = await _queryRepository.GetByIdAsync(dto.VerificationId, cancellationToken);
            if (verification == null || verification.IsUsed || verification.Expiration < DateTime.UtcNow)
            {
                _logger.LogWarning("Verification failed for id {Id}", dto.VerificationId);
                throw new NotFoundException(typeof(RegistrationVerification));
            }

            if (verification.Code != dto.Code)
            {
                _logger.LogWarning("Invalid code for verification id {Id}", dto.VerificationId);
                return new ConfirmationResultDto { Success = false, Error = "Invalid code." };
            }

            verification.IsUsed = true;
            verification.ConfirmedAt = DateTime.UtcNow;
            await _commandRepository.UpdateAsync(verification, cancellationToken);

            // Здесь можно создать пользователя, если требуется

            _logger.LogInformation("Verification succeeded for id {Id}", dto.VerificationId);

            return new ConfirmationResultDto { Success = true };
        }
    }
}
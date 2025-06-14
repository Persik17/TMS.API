using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.Models.DTOs.Registration;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for confirming registration by code.
    /// </summary>
    public class VerificationService : IVerificationService<RegistrationConfirmationDto, ConfirmationResultDto>
    {
        private readonly ICommandRepository<RegistrationVerification> _commandRepository;
        private readonly IQueryRepository<RegistrationVerification> _queryRepository;
        private readonly ILogger<VerificationService> _logger;

        public VerificationService(
            ICommandRepository<RegistrationVerification> commandRepository,
            IQueryRepository<RegistrationVerification> queryRepository,
            ILogger<VerificationService> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

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
using Microsoft.Extensions.Logging;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Factories;
using TMS.Application.Abstractions.Security;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Verification;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for confirming registration or authentication by code.
    /// </summary>
    public class VerificationService : IVerificationService
    {
        private readonly IUserVerificationRepository _userVerificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IColumnRepository _boardColumnRepository;
        private readonly IUserFactory _userFactory;
        private readonly ITokenService _tokenService;
        private readonly ILogger<VerificationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationService"/> class.
        /// </summary>
        /// <param name="queryRepository">The repository for performing registration verification queries (e.g., get by id).</param>
        /// <param name="commandRepository">The repository for performing registration verification commands (e.g., insert, update).</param>
        /// <param name="userRepository">The repository for accessing and updating user information.</param>
        /// <param name="logger">The logger for logging verification service events.</param>
        public VerificationService(
            IUserVerificationRepository userVerificationRepository,
            IUserRepository userRepository,
            IBoardRepository boardRepository,
            IColumnRepository boardColumnRepository,
            ITokenService tokenService,
            IUserFactory userFactory,
            ILogger<VerificationService> logger)
        {
            _userVerificationRepository = userVerificationRepository ?? throw new ArgumentNullException(nameof(userVerificationRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
            _boardColumnRepository = boardColumnRepository ?? throw new ArgumentNullException(nameof(boardColumnRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ConfirmationResultDto> ConfirmLoginAsync(ConfirmationDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                    return new ConfirmationResultDto { Success = false, Error = "Code is required." };

                var verification = await _userVerificationRepository.GetByIdAsync(dto.VerificationId, cancellationToken);

                if (verification == null)
                {
                    _logger.LogWarning("Verification record not found for id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Verification record not found." };
                }

                if (verification.IsUsed)
                {
                    _logger.LogWarning("Verification already used for id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Verification already used." };
                }

                if (verification.Expiration < DateTime.UtcNow)
                {
                    _logger.LogWarning("Verification expired for id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Verification expired." };
                }

                if (verification.Type != (int)VerificationType.Authentication)
                {
                    _logger.LogWarning("Verification type mismatch for id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Invalid verification type." };
                }

                if (verification.Code != dto.Code)
                {
                    _logger.LogWarning("Invalid code for verification id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Invalid code." };
                }

                verification.IsUsed = true;
                verification.ConfirmedAt = DateTime.UtcNow;
                await _userVerificationRepository.UpdateAsync(verification, cancellationToken);

                var user = await _userRepository.FindByEmailAsync(verification.Email, cancellationToken);

                if (user == null || user.Status != (int)UserStatus.Active)
                {
                    _logger.LogWarning("User is not active or not found: {Email}", verification.Email);
                    return new ConfirmationResultDto { Success = false, Error = "User is not active." };
                }

                var token = _tokenService.GenerateToken(user.ToUserDto());

                _logger.LogInformation("User logged in successfully, generating token.");

                return new ConfirmationResultDto { Success = true, Token = token };
            }
            catch (NotFoundException nfEx)
            {
                _logger.LogError(nfEx, "Unable to find the request verification with id {id}", dto.VerificationId);
                return new ConfirmationResultDto { Success = false, Error = "Unable to process the request" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during confirmation for id {Id}", dto.VerificationId);
                return new ConfirmationResultDto { Success = false, Error = "An unexpected error occurred." };
            }
        }

        /// <inheritdoc/>
        public async Task<ConfirmationResultDto> ConfirmRegistrationAsync(ConfirmationDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
            {
                _logger.LogWarning("Confirmation failed: dto is null.");
                return new ConfirmationResultDto { Success = false, Error = "Invalid confirmation data." };
            }

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                _logger.LogWarning("Confirmation failed: code is null or empty. VerificationId: {Id}", dto.VerificationId);
                return new ConfirmationResultDto { Success = false, Error = "Code is required." };
            }

            try
            {
                var verification = await _userVerificationRepository.GetByIdAsync(dto.VerificationId, cancellationToken);
                if (verification == null)
                {
                    _logger.LogWarning("Verification record not found for id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Verification record not found." };
                }

                if (verification.IsUsed)
                {
                    _logger.LogWarning("Verification already used for id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Verification already used." };
                }

                if (verification.Expiration < DateTime.UtcNow)
                {
                    _logger.LogWarning("Verification expired for id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Verification expired." };
                }

                if (verification.Type != (int)VerificationType.Registration)
                {
                    _logger.LogWarning("Verification type mismatch for id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Invalid verification type." };
                }

                if (verification.Code != dto.Code)
                {
                    _logger.LogWarning("Invalid code for verification id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Invalid code." };
                }

                verification.IsUsed = true;
                verification.ConfirmedAt = DateTime.UtcNow;
                await _userVerificationRepository.UpdateAsync(verification, cancellationToken);

                var user = await _userRepository.FindByEmailAsync(verification.Email, cancellationToken);
                if (user == null)
                {
                    _logger.LogWarning("User not found for email: {Email}", verification.Email);
                    return new ConfirmationResultDto { Success = false, Error = "User not found." };
                }

                if (user.Status == (int)UserStatus.Active)
                {
                    _logger.LogInformation("User already active for email: {Email}", user.Email);
                    return new ConfirmationResultDto { Success = false, Error = "User is already active." };
                }

                if (user.Status != (int)UserStatus.Pending)
                {
                    _logger.LogWarning("User not in pending status for email: {Email}, status: {Status}", user.Email, user.Status);
                    return new ConfirmationResultDto { Success = false, Error = "User is not eligible for activation." };
                }

                user.Status = (int)UserStatus.Active;
                await _userRepository.UpdateAsync(user, cancellationToken);

                var token = _tokenService.GenerateToken(user.ToUserDto());

                _logger.LogInformation("User {Email} activated and token generated.", user.Email);

                return new ConfirmationResultDto { Success = true, Token = token };
            }
            catch (NotFoundException nfEx)
            {
                _logger.LogError(nfEx, "Unable to find the request verification with id {id}", dto.VerificationId);
                return new ConfirmationResultDto { Success = false, Error = "Unable to process the request" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during confirmation for id {Id}", dto.VerificationId);
                return new ConfirmationResultDto { Success = false, Error = "An unexpected error occurred." };
            }
        }
    }
}
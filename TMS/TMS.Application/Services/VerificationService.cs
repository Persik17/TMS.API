using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Factories;
using TMS.Abstractions.Interfaces.Repositories;
using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Interfaces.Security;
using TMS.Abstractions.Interfaces.Services;
using TMS.Abstractions.Models.DTOs.Authentication;
using TMS.Abstractions.Models.DTOs.Registration;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for confirming registration or authentication by code.
    /// </summary>
    public class VerificationService : IVerificationService
    {
        private readonly IQueryRepository<UserVerification> _queryRepository;
        private readonly ICommandRepository<UserVerification> _commandRepository;
        private readonly IUserRepository<User> _userRepository;
        private readonly ICredentialRepository<Credential> _credentialRepository;
        private readonly IUserFactory _userFactory;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<VerificationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationService"/> class.
        /// </summary>
        /// <param name="queryRepository">The repository for performing registration verification queries (e.g., get by id).</param>
        /// <param name="commandRepository">The repository for performing registration verification commands (e.g., insert, update).</param>
        /// <param name="userRepository">The repository for accessing and updating user information.</param>
        /// <param name="logger">The logger for logging verification service events.</param>
        public VerificationService(
            IQueryRepository<UserVerification> queryRepository,
            ICommandRepository<UserVerification> commandRepository,
            IUserRepository<User> userRepository,
            IUserFactory userFactory,
            ILogger<VerificationService> logger)
        {
            _queryRepository = queryRepository ?? throw new ArgumentNullException(nameof(queryRepository));
            _commandRepository = commandRepository ?? throw new ArgumentNullException(nameof(commandRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ConfirmationResultDto> ConfirmLoginAsync(AuthenticationConfirmationDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            try
            {
                var verification = await _queryRepository.GetByIdAsync(dto.VerificationId, cancellationToken);

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

                if (verification.Code != dto.Code)
                {
                    _logger.LogWarning("Invalid code for verification id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Invalid code." };
                }

                verification.IsUsed = true;
                verification.ConfirmedAt = DateTime.UtcNow;
                await _commandRepository.UpdateAsync(verification, cancellationToken);

                // TODO: Generate JWT token here
                var token = "your_jwt_token_here"; // Replace with actual token generation

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
        public async Task<ConfirmationResultDto> ConfirmRegistrationAsync(RegistrationConfirmationDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            try
            {
                var verification = await _queryRepository.GetByIdAsync(dto.VerificationId, cancellationToken);
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

                if (verification.Code != dto.Code)
                {
                    _logger.LogWarning("Invalid code for verification id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "Invalid code." };
                }

                verification.IsUsed = true;
                verification.ConfirmedAt = DateTime.UtcNow;
                await _commandRepository.UpdateAsync(verification, cancellationToken);

                var user = await _userRepository.FindByEmailAsync(verification.Email, cancellationToken);

                if (user != null)
                {
                    _logger.LogWarning("User already exists for verification id {Id}", dto.VerificationId);
                    return new ConfirmationResultDto { Success = false, Error = "User already exists." };
                }

                // TODO: Extract user creation logic into a separate service or method
                // For example, create a new UserCreateDto and pass it to a new method like this
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.UtcNow,
                    RegistrationDate = DateTime.UtcNow,
                    LastLoginDate = DateTime.UtcNow,
                    Email = verification.Email,
                    Status = 0,
                    IsVerified = false,
                    Timezone = "Europe/Kazan",
                    Language = "ru"
                };

                await _userRepository.InsertAsync(newUser, cancellationToken);

                // Создание кредов
                string salt = _passwordHasher.GenerateSalt();
                //string passwordHash = _passwordHasher.Hash(dto.Password, salt);

                var credential = new Credential
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.UtcNow,
                    UserId = newUser.Id,
                    Email = newUser.Email,
                    //PasswordHash = passwordHash,
                    PasswordSalt = salt,
                    // Остальные поля можно не заполнять (ResetToken, ResetTokenExpiration)
                };

                await _credentialRepository.InsertAsync(credential, cancellationToken);

                // TODO: Generate JWT token here
                var token = "your_jwt_token_here"; // Replace with actual token generation

                _logger.LogInformation("Registration succeeded, generating token.");

                return new ConfirmationResultDto { Success = true, Token = token };
            }
            catch (NotFoundException nfEx)
            {
                _logger.LogError(nfEx, "Unable to find the request verification with id {id}", dto.VerificationId);
                return new ConfirmationResultDto { Success = false, Error = "Unable to process the request" };
            }
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "An unexpected error occurred during confirmation for id {Id}", dto.VerificationId);
            //    return new ConfirmationResultDto { Success = false, Error = "An unexpected error occurred." };
            //}
        }
    }
}
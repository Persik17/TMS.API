using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using TMS.Abstractions.Enums;
using TMS.Application.Abstractions.Factories;
using TMS.Application.Abstractions.Messaging;
using TMS.Application.Abstractions.Security;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Registration;
using TMS.Contracts.Events;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for registering new users and initiating verification.
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserVerificationRepository _userVerificationRepository;
        private readonly ICredentialRepository _credentialRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;
        private readonly IPasswordHasher _passwordHasher;
        private readonly INotifyService _notifyService;
        private readonly ILogger<RegistrationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationService"/> class.
        /// </summary>
        /// <param name="commandRepository">The repository for performing registration verification commands (e.g., insert).</param>
        /// <param name="notifyService">The service for sending notifications (e.g., email, SMS, Telegram).</param>
        /// <param name="logger">The logger for logging registration service events.</param>
        public RegistrationService(
            IUserVerificationRepository userVerificationRepository,
            ICredentialRepository credentialRepository,
            IUserRepository userRepository,
            IUserFactory userFactory,
            IPasswordHasher passwordHasher,
            INotifyService notifyService,
            ILogger<RegistrationService> logger)
        {
            _userVerificationRepository = userVerificationRepository ?? throw new ArgumentNullException(nameof(userVerificationRepository));
            _credentialRepository = credentialRepository ?? throw new ArgumentNullException(nameof(credentialRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<RegistrationResultDto> RegisterAsync(RegistrationDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
            {
                _logger.LogWarning("Registration failed: dto is null.");
                return new RegistrationResultDto { Success = false, Error = "Invalid registration data." };
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                _logger.LogWarning("Registration failed: Email is null or empty.");
                return new RegistrationResultDto { Success = false, Error = "Email is required." };
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                _logger.LogWarning("Registration failed: Password is null or empty, email: {Email}", dto.Email);
                return new RegistrationResultDto { Success = false, Error = "Password is required." };
            }

            try
            {
                dto.Email = dto.Email.Trim();

                try
                {
                    var addr = new System.Net.Mail.MailAddress(dto.Email);
                }
                catch
                {
                    _logger.LogWarning("Registration failed: Invalid email format: {Email}", dto.Email);
                    return new RegistrationResultDto { Success = false, Error = "Invalid email format." };
                }

                var existingUser = await _userRepository.FindByEmailAsync(dto.Email, cancellationToken);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: email already exists: {Email}", dto.Email);
                    return new RegistrationResultDto { Success = false, Error = "User with this email already exists." };
                }

                if (dto.Password.Length < 8)
                {
                    _logger.LogWarning("Registration failed: password too short for email: {Email}", dto.Email);
                    return new RegistrationResultDto { Success = false, Error = "Password must be at least 8 characters." };
                }

                var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

                //TODO сделать транзакцию, предварительно сделав unitOfWork
                // using var transaction = await _userRepository.BeginTransactionAsync(cancellationToken);

                var verification = new UserVerification
                {
                    Id = Guid.NewGuid(),
                    Email = dto.Email,
                    Code = code,
                    Expiration = DateTime.UtcNow.AddMinutes(10),
                    IsUsed = false,
                    Type = (int)VerificationType.Registration,
                    CreatedAt = DateTime.UtcNow
                };
                await _userVerificationRepository.InsertAsync(verification, cancellationToken);

                var newUser = _userFactory.CreatePendingUserByEmail(verification.Email);
                await _userRepository.InsertAsync(newUser, cancellationToken);

                string salt = _passwordHasher.GenerateSalt();
                var passwordHash = _passwordHasher.Hash(dto.Password, salt);

                var credential = new Credential
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.UtcNow,
                    UserId = newUser.Id,
                    Email = newUser.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = salt
                };
                await _credentialRepository.InsertAsync(credential, cancellationToken);

                //await _notifyService.PublishAsync(new UserVerificationCreatedEvent
                //{
                //    VerificationId = verification.Id,
                //    Target = verification.Email,
                //    Code = verification.Code,
                //    Expiration = verification.Expiration,
                //    Message = "Ваш код подтверждения"
                //}, cancellationToken);

                // await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Registration verification created for {Target}", dto.Email);

                return new RegistrationResultDto
                {
                    VerificationId = verification.Id,
                    Expiration = verification.Expiration,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed: unhandled exception for email: {Email}", dto?.Email);
                // await transaction?.RollbackAsync(cancellationToken);
                return new RegistrationResultDto { Success = false, Error = "Registration failed due to an internal error." };
            }
        }
    }
}
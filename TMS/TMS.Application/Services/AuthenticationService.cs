using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories;
using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Interfaces.Security;
using TMS.Abstractions.Interfaces.Services;
using TMS.Abstractions.Models.DTOs.Authentication;
using TMS.Contracts.Events;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for authenticating users.
    /// Provides login logic, including user lookup, password verification, and token generation.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICredentialRepository<Credential> _credentialRepository;
        private readonly ICommandRepository<UserVerification> _commandRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly INotifyService _notifyService;
        private readonly ILogger<AuthenticationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="credentialRepository">The repository for accessing user credentials.</param>
        /// <param name="commandRepository">The repository for managing user verification entities.</param>
        /// <param name="passwordHasher">The password hashing service.</param>
        /// <param name="notifyService">The notification publishing service.</param>
        /// <param name="logger">The logger for logging authentication events.</param>
        public AuthenticationService(
            ICredentialRepository<Credential> credentialRepository,
            ICommandRepository<UserVerification> commandRepository,
            IPasswordHasher passwordHasher,
            INotifyService notifyService,
            ILogger<AuthenticationService> logger)
        {
            _credentialRepository = credentialRepository ?? throw new ArgumentNullException(nameof(credentialRepository));
            _commandRepository = commandRepository ?? throw new ArgumentNullException(nameof(commandRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResultDto> AuthenticateAsync(AuthenticationDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            _logger.LogInformation("Attempting to authenticate user with email: {Email}", dto.Email);

            var credential = await _credentialRepository.GetByEmailAsync(dto.Email, cancellationToken);
            if (credential == null)
            {
                _logger.LogWarning("Authentication failed for email: {Email}", dto.Email);
                throw new NotFoundException(typeof(Credential));
            }

            if (!_passwordHasher.Verify(credential.PasswordHash, dto.Password, credential.PasswordSalt))
            {
                _logger.LogWarning("Authentication failed for email: {Email}", dto.Email);
                throw new AuthenticationFailedException("Invalid email or password.");
            }

            _logger.LogInformation("Creating a verification request for User {Email}", dto.Email);

            var code = new Random().Next(100000, 999999).ToString();
            var verification = new UserVerification
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                Code = code,
                Expiration = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _commandRepository.InsertAsync(verification, cancellationToken);

            await _notifyService.PublishAsync(new RegistrationVerificationCreatedEvent
            {
                VerificationId = verification.Id,
                Target = verification.Email,
                Type = verification.Type,
                Code = verification.Code,
                Expiration = verification.Expiration,
                Message = ""
            }, cancellationToken);

            return new AuthenticationResultDto
            {
                Success = true,
                VerificationId = verification.Id
            };
        }
    }
}
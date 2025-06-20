using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories;
using TMS.Abstractions.Interfaces.Security;
using TMS.Abstractions.Interfaces.Services;
using TMS.Abstractions.Models.DTOs.Authentication;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for authenticating users.
    /// Provides login logic, including user lookup, password verification, and token generation.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly ICredentialRepository<Credential> _credentialRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthenticationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="userRepository">The repository for accessing user data.</param>
        /// <param name="credentialRepository">The repository for accessing user credentials.</param>
        /// <param name="passwordHasher">The password hashing service.</param>
        /// <param name="logger">The logger for logging authentication events.</param>
        public AuthenticationService(
            IUserRepository<User> userRepository,
            ICredentialRepository<Credential> credentialRepository,
            IPasswordHasher passwordHasher,
            ILogger<AuthenticationService> logger
        )
        {
            _userRepository = userRepository;
            _credentialRepository = credentialRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResultDto> AuthenticateAsync(AuthenticationDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            _logger.LogInformation("Attempting to authenticate user with email: {Email}", dto.Email);

            // Example check (replace with your user lookup and password verification logic)
            var user = await _userRepository.FindByEmailAsync(dto.Email, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("Authentication failed for email: {Email}", dto.Email);
                throw new NotFoundException(typeof(User));
            }

            var credential = await _credentialRepository.GetByUserIdAsync(user.Id, cancellationToken);
            if (credential == null || !_passwordHasher.Verify(credential.PasswordHash, dto.Password, credential.PasswordSalt))
            {
                _logger.LogWarning("Authentication failed for email: {Email}", dto.Email);
                throw new AuthenticationFailedException("Invalid email or password.");
            }

            // Example successful authentication (replace with your token generation logic)
            var result = new AuthenticationResultDto
            {
                Success = true,
                Token = "jwt-token", // Generate a real token
                Error = null
            };

            _logger.LogInformation("User with email {Email} authenticated successfully", dto.Email);

            return result; // Removed unnecessary Task.FromResult
        }
    }
}
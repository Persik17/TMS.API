using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories;
using TMS.Abstractions.Interfaces.Security;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.Models.DTOs.Authentication;
using TMS.Infrastructure.DataAccess.DataModels;

using Task = System.Threading.Tasks.Task;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for authenticating users.
    /// Provides login logic, logging, and error handling.
    /// </summary>
    public class AuthenticationService : IAuthenticationService<AuthenticationDto, AuthenticationResultDto>
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly ICredentialRepository<Credential> _credentialRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IUserRepository<User> userRepository,
            ICredentialRepository<Credential> credentialRepository,
            IPasswordHasher passwordHasher,
            ILogger<AuthenticationService> logger
        )
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResultDto> AuthenticateAsync(AuthenticationDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            _logger.LogInformation("Attempting to authenticate user with email: {Email}", dto.Email);

            // Пример проверки (замени на свою логику поиска пользователя и проверки пароля)
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

            // Пример успешной аутентификации (замени на свою логику генерации токена)
            var result = new AuthenticationResultDto
            {
                Success = true,
                Token = "jwt-token", // Генерируй реальный токен
                Error = null
            };

            _logger.LogInformation("User with email {Email} authenticated successfully", dto.Email);

            return await Task.FromResult(result);
        }
    }
}
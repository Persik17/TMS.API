using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Factories;
using TMS.Application.Abstractions.Messaging;
using TMS.Application.Abstractions.Security;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Authentication;
using TMS.Application.Extensions;
using TMS.Application.Security;
using TMS.Contracts.Events;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for authenticating users.
    /// Provides login logic, including user lookup, password verification, and token generation.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICredentialRepository _credentialRepository;
        private readonly IUserVerificationRepository _userVerificationRepository;
        private readonly ITelegramAccountRepository _telegramAccountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly INotifyService _notifyService;
        private readonly ILogger<AuthenticationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="credentialRepository">The repository for accessing user credentials.</param>
        /// <param name="passwordHasher">The password hashing service.</param>
        /// <param name="notifyService">The notification publishing service.</param>
        /// <param name="logger">The logger for logging authentication events.</param>
        public AuthenticationService(
            ICredentialRepository credentialRepository,
            IUserVerificationRepository userVerificationRepository,
            ITelegramAccountRepository telegramAccountRepository,
            IUserRepository userRepository,
            IUserFactory userFactory,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            INotifyService notifyService,
            ILogger<AuthenticationService> logger)
        {
            _credentialRepository = credentialRepository ?? throw new ArgumentNullException(nameof(credentialRepository));
            _userVerificationRepository = userVerificationRepository ?? throw new ArgumentNullException(nameof(userVerificationRepository));
            _telegramAccountRepository = telegramAccountRepository ?? throw new ArgumentNullException(nameof(telegramAccountRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResultDto> AuthenticateAsync(AuthenticationDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));
                if (string.IsNullOrWhiteSpace(dto.Email))
                    throw new ArgumentException("Email is required.", nameof(dto.Email));
                if (string.IsNullOrWhiteSpace(dto.Password))
                    throw new ArgumentException("Password is required.", nameof(dto.Password));

                dto.Email = dto.Email.Trim();

                try
                {
                    var addr = new System.Net.Mail.MailAddress(dto.Email);
                }
                catch
                {
                    _logger.LogWarning("Invalid email format: {Email}", dto.Email);
                    throw new AuthenticationFailedException("Invalid email or password.");
                }

                _logger.LogInformation("Attempting to authenticate user with email: {Email}", dto.Email);

                var credential = await _credentialRepository.GetByEmailAsync(dto.Email, cancellationToken);
                if (credential == null)
                {
                    _logger.LogWarning("Authentication failed for email: {Email}", dto.Email);
                    throw new AuthenticationFailedException("Invalid email or password.");
                }

                var user = await _userRepository.GetByIdAsync(credential.UserId, cancellationToken);
                if (user == null || user.Status != (int)UserStatus.Active)
                {
                    _logger.LogWarning("Authentication failed: user inactive or not found. Email: {Email}", dto.Email);
                    throw new AuthenticationFailedException("User is not active.");
                }

                if (!_passwordHasher.Verify(credential.PasswordHash, dto.Password, credential.PasswordSalt))
                {
                    _logger.LogWarning("Authentication failed for email: {Email}", dto.Email);
                    throw new AuthenticationFailedException("Invalid email or password.");
                }

                _logger.LogInformation("Creating a verification request for User {Email}", dto.Email);

                var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
                var verification = new UserVerification
                {
                    Id = Guid.NewGuid(),
                    Email = dto.Email,
                    Code = code,
                    Expiration = DateTime.UtcNow.AddMinutes(10),
                    IsUsed = false,
                    Type = (int)VerificationType.Authentication,
                    CreatedAt = DateTime.UtcNow
                };

                await _userVerificationRepository.InsertAsync(verification, cancellationToken);

                //await _notifyService.PublishAsync(new UserVerificationCreatedEvent
                //{
                //    VerificationId = verification.Id,
                //    Target = verification.Email,
                //    Type = verification.Type,
                //    Code = verification.Code,
                //    Expiration = verification.Expiration,
                //    Message = "Ваш код подтверждения"
                //}, cancellationToken);

                return new AuthenticationResultDto
                {
                    Success = true,
                    VerificationId = verification.Id,
                    Expiration = verification.Expiration
                };
            }
            catch (AuthenticationFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during authentication.");
                throw new AuthenticationFailedException("Authentication failed due to internal error.");
            }
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResultDto> AuthenticateTelegramAsync(TelegramAuthenticationDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Telegram DTO is null.");

            if (dto.TelegramUserId == 0)
                throw new AuthenticationFailedException("Telegram user id is required.");

            if (dto.AuthDate == default)
                throw new AuthenticationFailedException("Telegram authentication date is required.");

            try
            {
                if (!TelegramSignatureVerifier.Verify(dto))
                {
                    _logger.LogWarning("Telegram signature verification failed for userId: {Id}", dto.TelegramUserId);
                    throw new AuthenticationFailedException("Invalid Telegram signature.");
                }

                var telegramAccount = await _telegramAccountRepository.GetByTelegramUserIdAsync(dto.TelegramUserId, cancellationToken);

                User? user = null;
                if (telegramAccount != null)
                {
                    user = await _userRepository.GetByTelegramAccountIdAsync(telegramAccount.Id, cancellationToken);
                    if (user == null)
                    {
                        _logger.LogError("TelegramAccount {Id} exists but user not found.", telegramAccount.Id);
                        throw new AuthenticationFailedException("Internal error. Please try again later.");
                    }
                }
                else
                {
                    telegramAccount = new TelegramAccount
                    {
                        Id = Guid.NewGuid(),
                        CreationDate = DateTime.UtcNow,
                        Username = dto.Username,
                        FullName = dto.FullName,
                        Phone = dto.Phone,
                        TelegramUserId = dto.TelegramUserId,
                        AuthDate = dto.AuthDate,
                        Hash = dto.Hash
                    };

                    await _telegramAccountRepository.InsertAsync(telegramAccount, cancellationToken);

                    user = _userFactory.CreateUserFromTelegram(dto, telegramAccount.Id);
                    await _userRepository.InsertAsync(user, cancellationToken);
                }

                var token = _tokenService.GenerateToken(user.ToUserDto());

                return new AuthenticationResultDto
                {
                    Success = true,
                    Token = token,
                    UserId = user.Id,
                    TelegramId = telegramAccount.Id,
                    FullName = user.FullName,
                    Email = user.Email
                };
            }
            catch (AuthenticationFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Telegram authentication for userId: {Id}", dto.TelegramUserId);
                throw new AuthenticationFailedException("Internal error. Please try again later.");
            }
        }
    }
}
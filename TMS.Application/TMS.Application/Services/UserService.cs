using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.TelegramAccount;
using TMS.Application.Dto.User;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

using Task = System.Threading.Tasks.Task;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing User entities.
    /// Provides CRUD operations and uses logging for diagnostics and monitoring.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITelegramAccountRepository _telegramAccountCommandRepository;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The repository for accessing user data.</param>
        /// <param name="telegramAccountCommandRepository">The repository for performing auditable Telegram account commands (e.g., insert, update).</param>
        /// <param name="logger">The logger for logging user service events.</param>
        public UserService(
            IUserRepository userRepository,
            ITelegramAccountRepository telegramAccountCommandRepository,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _telegramAccountCommandRepository = telegramAccountCommandRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<UserDto> CreateAsync(UserCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new user: {FullName}", createDto.FullName);

            var newUser = createDto.ToUser();

            await _userRepository.InsertAsync(newUser, cancellationToken);

            var createdUser = await _userRepository.GetByIdAsync(newUser.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(User));

            _logger.LogInformation("User created successfully with id: {Id}", newUser.Id);

            return createdUser.ToUserDto();
        }

        /// <inheritdoc/>
        public async Task<UserDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get user with empty id");
                throw new WrongIdException(typeof(User));
            }

            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null)
                _logger.LogWarning("User with id {Id} not found", id);

            return user?.ToUserDto();
        }

        /// <inheritdoc/>
        public async Task<UserDto> UpdateAsync(UserDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.Id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to update user with empty id");
                throw new WrongIdException(typeof(User));
            }

            var existingUser = await _userRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existingUser == null)
            {
                _logger.LogWarning("User with id {Id} not found for update", dto.Id);
                throw new NotFoundException(typeof(User));
            }

            existingUser.FullName = dto.FullName;
            existingUser.Email = dto.Email;
            existingUser.TelegramId = dto.TelegramId;
            existingUser.Timezone = dto.Timezone;
            existingUser.Language = dto.Language;
            existingUser.Phone = dto.Phone;
            existingUser.Status = dto.Status;
            existingUser.NotificationSettingsId = dto.NotificationSettingsId;
            existingUser.LastLoginDate = dto.LastLoginDate;
            existingUser.CreationDate = dto.CreationDate;
            existingUser.UpdateDate = dto.UpdateDate;
            existingUser.DeleteDate = dto.DeleteDate;

            await _userRepository.UpdateAsync(existingUser, cancellationToken);

            _logger.LogInformation("User with id {Id} updated successfully", dto.Id);

            return dto;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete user with empty id");
                throw new WrongIdException(typeof(User));
            }

            await _userRepository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("User with id {Id} deleted", id);
        }

        /// <inheritdoc/>
        public async Task LinkTelegramAccountAsync(Guid userId, TelegramAccountCreateDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException(typeof(User));

            // Если уже привязан, можно выбросить ошибку или обновить TelegramAccount
            if (user.TelegramId != null)
            {
                _logger.LogWarning("User {UserId} already has a linked TelegramAccount", userId);
                throw new InvalidOperationException("Telegram account already linked.");
            }

            var telegramAccount = new TelegramAccount
            {
                Id = Guid.NewGuid(),
                Username = dto.NickName,
                Phone = dto.Phone,
                CreationDate = DateTime.UtcNow
            };

            await _telegramAccountCommandRepository.InsertAsync(telegramAccount, cancellationToken);

            user.TelegramId = telegramAccount.Id;
            await _userRepository.UpdateAsync(user, cancellationToken);

            _logger.LogInformation("TelegramAccount {TelegramId} linked to user {UserId}", telegramAccount.Id, userId);
        }

        /// <inheritdoc/>
        public async Task UnlinkTelegramAccountAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException(typeof(User));

            if (user.TelegramId == null)
            {
                _logger.LogWarning("User {UserId} has no linked TelegramAccount", userId);
                throw new InvalidOperationException("No Telegram account linked.");
            }

            var telegramId = user.TelegramId.Value;

            await _telegramAccountCommandRepository.DeleteAsync(telegramId, cancellationToken);

            user.TelegramId = null;
            await _userRepository.UpdateAsync(user, cancellationToken);

            _logger.LogInformation("TelegramAccount {TelegramId} unlinked from user {UserId}", telegramId, userId);
        }
    }
}
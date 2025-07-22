using Microsoft.Extensions.Logging;
using TMS.Abstractions.Enums;
using TMS.Application.Abstractions.Services;
using TMS.Infrastructure.Abstractions.Repositories;

namespace TMS.Application.Security
{
    public class AccessService : IAccessService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<AccessService> _logger;

        public AccessService(
            IUserRepository userRepository,
            IBoardRepository boardRepository,
            ICompanyRepository companyRepository,
            IRoleRepository roleRepository,
            ILogger<AccessService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> HasPermissionAsync(
            Guid userId,
            Guid resourceId,
            ResourceType resourceType,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found", userId);
                return false;
            }

            var role = await _roleRepository.GetByIdAsync((Guid)user.RoleId);
            if (role == null)
            {
                _logger.LogWarning("User {UserId} has no role", userId);
                return false;
            }

            if (role.Name.Equals("Owner", StringComparison.OrdinalIgnoreCase) ||
                role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Owner и Admin видят всё
                return true;
            }

            // 3. Пользователь — проверяем доступ к ресурсу
            switch (resourceType)
            {
                case ResourceType.Company:
                    var company = await _companyRepository.GetByIdAsync(resourceId, cancellationToken);
                    if (company == null) return false;
                    if (company.OwnerId == user.Id) return true;
                    break;

                case ResourceType.Board:
                    var board = await _boardRepository.GetByIdAsync(resourceId, cancellationToken);
                    if (board == null) return false;
                    if (board.HeadId == user.Id) return true;
                    if (board.Users.Any(u => u.Id == user.Id)) return true;
                    break;

                case ResourceType.Column:
                case ResourceType.User:
                    break;
            }

            // Если ничего не совпало — нет доступа
            return false;
        }
    }
}
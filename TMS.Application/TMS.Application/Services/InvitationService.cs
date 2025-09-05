using Microsoft.Extensions.Logging;
using TMS.Abstractions.Enums;
using TMS.Application.Abstractions.Messaging;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.User;
using TMS.Contracts.Events;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IUserInvitationRepository _invitationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly INotifyService _notifyService;
        private readonly ILogger<InvitationService> _logger;

        public InvitationService(
            IUserRepository userRepository,
            IBoardRepository boardRepository,
            IUserInvitationRepository invitationRepository,
            IRoleRepository roleRepository,
            INotifyService notifyService,
            ILogger<InvitationService> logger)
        {
            _userRepository = userRepository;
            _boardRepository = boardRepository;
            _invitationRepository = invitationRepository;
            _roleRepository = roleRepository;
            _notifyService = notifyService;
            _logger = logger;
        }

        public async System.Threading.Tasks.Task InviteByEmailAsync(UserInviteDto dto, Guid adminId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var existingUser = await _userRepository.FindByEmailAsync(dto.Email, cancellationToken);
            if (existingUser != null)
            {
                if (existingUser.Status != (int)UserStatus.Invited)
                    throw new InvalidOperationException("User with this email already exists and is active.");
            }
            else
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Language = dto.Language,
                    Status = (int)UserStatus.Invited,
                    CreationDate = DateTime.UtcNow
                };
                await _userRepository.InsertAsync(user, cancellationToken);
                existingUser = user;

                foreach (var roleName in dto.Roles)
                {
                    var role = await _roleRepository.GetByNameAsync(roleName, cancellationToken);
                    if (role == null)
                        throw new Exception($"Role '{roleName}' not found");
                }
            }

            var boardUser = new BoardUser
            {
                BoardsId = dto.BoardId,
                UsersId = existingUser.Id
            };
            await _boardRepository.AddUserToBoardAsync(dto.BoardId, existingUser.Id, cancellationToken);

            var invitation = new UserInvitation
            {
                Id = Guid.NewGuid(),
                UserId = existingUser.Id,
                Email = dto.Email,
                FullName = dto.FullName,
                Roles = dto.Roles,
                Language = dto.Language,
                InvitedBy = adminId,
                CreatedAt = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(3),
                Status = (int)InvitationStatus.Pending,
                CustomMessage = dto.CustomMessage
            };
            await _invitationRepository.InsertAsync(invitation, cancellationToken);

            var inviteEvent = new UserInvitationCreatedEvent
            {
                InvitationId = invitation.Id,
                UserId = invitation.UserId,
                Email = invitation.Email,
                FullName = invitation.FullName,
                Roles = invitation.Roles,
                Language = invitation.Language,
                Message = dto.CustomMessage ?? "You have been invited to join our platform.",
                InvitedBy = invitation.InvitedBy,
                Expiration = invitation.Expiration
            };
            await _notifyService.PublishAsync(inviteEvent, cancellationToken);

            _logger.LogInformation("Invitation for user {Email} has been created and published by {AdminId}", invitation.Email, adminId);
        }

        public async System.Threading.Tasks.Task AcceptInvitationAsync(Guid invitationId, string password, CancellationToken cancellationToken = default)
        {
            var invitation = await _invitationRepository.GetByIdAsync(invitationId, cancellationToken)
                ?? throw new Exception("Invitation not found");
            if (invitation.Status != (int)InvitationStatus.Pending || invitation.Expiration < DateTime.UtcNow)
                throw new InvalidOperationException("Invitation is not valid or already expired.");

            var user = await _userRepository.GetByIdAsync(invitation.UserId, cancellationToken)
                ?? throw new Exception("User not found");
            user.Status = (int)UserStatus.Active;
            // user.PasswordHash = _passwordHasher.HashPassword(password); // Вставь свою реализацию
            user.UpdateDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user, cancellationToken);

            invitation.Status = (int)InvitationStatus.Accepted;
            await _invitationRepository.UpdateAsync(invitation, cancellationToken);

            _logger.LogInformation("Invitation {InvitationId} accepted and user {UserId} activated.", invitationId, user.Id);
        }
    }
}

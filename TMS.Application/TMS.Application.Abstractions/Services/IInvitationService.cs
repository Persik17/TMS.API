using TMS.Application.Dto.User;

namespace TMS.Application.Abstractions.Services
{
    public interface IInvitationService
    {
        Task InviteByEmailAsync(UserInviteDto dto, Guid adminId, CancellationToken cancellationToken = default);
        Task AcceptInvitationAsync(Guid invitationId, string password, CancellationToken cancellationToken = default);
    }
}

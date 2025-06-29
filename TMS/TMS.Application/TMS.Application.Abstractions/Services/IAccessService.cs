using TMS.Abstractions.Enums;

namespace TMS.Application.Abstractions.Services
{
    public interface IAccessService
    {
        Task<bool> HasPermissionAsync(
            Guid userId,
            Guid resourceId,
            ResourceType resourceType,
            string permissionName,
            CancellationToken cancellationToken = default);
    }
}

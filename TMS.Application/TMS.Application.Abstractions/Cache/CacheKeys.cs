using TMS.Abstractions.Enums;

namespace TMS.Application.Abstractions.Cache
{
    public static class CacheKeys
    {
        public static string Company(Guid companyId) => $"company:{companyId}";
        public static string User(Guid userId) => $"user:{userId}";
        public static string Permissions(Guid userId, ResourceType resourceType, Guid resourceId) => $"permissions:{userId}:{resourceType}:{resourceId}";
    }
}

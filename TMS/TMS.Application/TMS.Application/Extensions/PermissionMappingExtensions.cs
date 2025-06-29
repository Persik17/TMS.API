using TMS.Application.Dto.Permission;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Extensions
{
    public static class PermissionMappingExtensions
    {
        public static Permission ToPermission(this PermissionCreateDto dto)
        {
            return new Permission
            {
                Name = dto.Name,
                Description = dto.Description,
                CreationDate = DateTime.UtcNow
            };
        }

        public static PermissionDto ToPermissionDto(this Permission entity)
        {
            return new PermissionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }
    }
}
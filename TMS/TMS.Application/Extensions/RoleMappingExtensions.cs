using TMS.Application.Models.DTOs.Role;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Extensions
{
    public static class RoleMappingExtensions
    {
        public static Role ToRole(this RoleCreateDto dto)
        {
            return new Role
            {
                Name = dto.Name,
                Description = dto.Description,
                CreationDate = DateTime.UtcNow
            };
        }

        public static RoleDto ToRoleDto(this Role entity)
        {
            return new RoleDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }
    }
}
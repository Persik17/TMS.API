using TMS.Application.DTOs.Department;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Extensions
{
    public static class DepartmentMappingExtensions
    {
        public static Department ToDepartment(this DepartmentCreateDto dto)
        {
            return new Department
            {
                Name = dto.Name,
                CompanyId = dto.CompanyId,
                HeadId = dto.HeadId,
                Description = dto.Description,
                IsActive = dto.IsActive,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                CreationDate = DateTime.UtcNow
            };
        }

        public static DepartmentDto ToDepartmentDto(this Department entity)
        {
            return new DepartmentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                HeadId = entity.HeadId,
                Description = entity.Description,
                IsActive = entity.IsActive,
                ContactEmail = entity.ContactEmail,
                ContactPhone = entity.ContactPhone,
                CreationDate = entity.CreationDate,
                UpdateDate = entity.UpdateDate,
                DeleteDate = entity.DeleteDate
            };
        }

        public static void UpdateFromDto(this Department entity, DepartmentDto dto)
        {
            entity.Name = dto.Name;
            entity.CompanyId = dto.CompanyId;
            entity.HeadId = dto.HeadId;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.ContactEmail = dto.ContactEmail;
            entity.ContactPhone = dto.ContactPhone;
            entity.CreationDate = dto.CreationDate;
            entity.UpdateDate = dto.UpdateDate;
            entity.DeleteDate = dto.DeleteDate;
        }
    }
}

using TMS.Application.Models.DTOs.Company;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Extensions
{
    public static class CompanyMappingExtensions
    {
        public static Company ToCompany(this CompanyCreateDto dto)
        {
            return new Company
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                INN = dto.INN,
                OGRN = dto.OGRN,
                Address = dto.Address,
                Logo = dto.Logo,
                Website = dto.Website,
                Industry = dto.Industry,
                Description = dto.Description,
                IsActive = dto.IsActive,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                CreationDate = DateTime.UtcNow
            };
        }

        public static CompanyDto ToCompanyDto(this Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                INN = company.INN,
                OGRN = company.OGRN,
                Address = company.Address,
                Logo = company.Logo,
                Website = company.Website,
                Industry = company.Industry,
                Description = company.Description,
                IsActive = company.IsActive,
                ContactEmail = company.ContactEmail,
                ContactPhone = company.ContactPhone,
                CreationDate = company.CreationDate,
                UpdateDate = company.UpdateDate,
                DeleteDate = company.DeleteDate
            };
        }
    }
}

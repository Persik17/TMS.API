using TMS.Application.Dto.Company;
using TMS.Infrastructure.DataModels;

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
                OwnerId = dto.OwnerId,
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
                OwnerId = company.OwnerId,
                ContactEmail = company.ContactEmail,
                ContactPhone = company.ContactPhone,
                CreationDate = company.CreationDate,
                UpdateDate = company.UpdateDate,
                DeleteDate = company.DeleteDate
            };
        }

        public static void UpdateFromDto(this Company company, CompanyDto dto)
        {
            company.Name = dto.Name;
            company.INN = dto.INN;
            company.OGRN = dto.OGRN;
            company.Address = dto.Address;
            company.Logo = dto.Logo;
            company.Website = dto.Website;
            company.Industry = dto.Industry;
            company.Description = dto.Description;
            company.OwnerId = dto.OwnerId;
            company.ContactEmail = dto.ContactEmail;
            company.ContactPhone = dto.ContactPhone;
            company.CreationDate = dto.CreationDate;
            company.UpdateDate = dto.UpdateDate;
            company.DeleteDate = dto.DeleteDate;
        }
    }
}

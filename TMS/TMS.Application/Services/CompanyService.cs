using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.Extensions;
using TMS.Application.Models.DTOs.Company;
using TMS.Infrastructure.DataAccess.DataModels;

using Task = System.Threading.Tasks.Task;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing Company entities.
    /// Provides CRUD operations and uses logging for diagnostics and monitoring.
    /// </summary>
    public class CompanyService : ICompanyService<CompanyDto, CompanyCreateDto>
    {
        private readonly IAuditableCommandRepository<Company> _commandRepository;
        private readonly IAuditableQueryRepository<Company> _queryRepository;
        private readonly ILogger<CompanyService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyService"/> class.
        /// </summary>
        /// <param name="commandRepository">Repository for write operations.</param>
        /// <param name="queryRepository">Repository for read operations.</param>
        /// <param name="logger">Logger instance for diagnostics.</param>
        public CompanyService(
            IAuditableCommandRepository<Company> commandRepository,
            IAuditableQueryRepository<Company> queryRepository,
            ILogger<CompanyService> logger)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<CompanyDto> CreateAsync(CompanyCreateDto createDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new company: {Name}", createDto.Name);

            var newCompany = createDto.ToCompany();
            newCompany.Id = Guid.NewGuid();

            await _commandRepository.InsertAsync(newCompany, cancellationToken);

            var createdCompany = await _queryRepository.GetByIdAsync(newCompany.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Company));

            _logger.LogInformation("Company created successfully with id: {Id}", newCompany.Id);

            return createdCompany.ToCompanyDto();
        }

        /// <inheritdoc/>
        public async Task<CompanyDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get company with empty id");
                throw new WrongIdException(typeof(Company));
            }

            var company = await _queryRepository.GetByIdAsync(id, cancellationToken);
            if (company == null)
                _logger.LogWarning("Company with id {Id} not found", id);

            return company?.ToCompanyDto();
        }

        /// <inheritdoc/>
        public async Task<CompanyDto> UpdateAsync(CompanyDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.Id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to update company with empty id");
                throw new WrongIdException(typeof(Company));
            }

            var existingCompany = await _queryRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existingCompany == null)
            {
                _logger.LogWarning("Company with id {Id} not found for update", dto.Id);
                throw new NotFoundException(typeof(Company));
            }

            existingCompany.Name = dto.Name;
            existingCompany.INN = dto.INN;
            existingCompany.OGRN = dto.OGRN;
            existingCompany.Address = dto.Address;
            existingCompany.Logo = dto.Logo;
            existingCompany.Website = dto.Website;
            existingCompany.Industry = dto.Industry;
            existingCompany.Description = dto.Description;
            existingCompany.IsActive = dto.IsActive;
            existingCompany.ContactEmail = dto.ContactEmail;
            existingCompany.ContactPhone = dto.ContactPhone;
            existingCompany.CreationDate = dto.CreationDate;
            existingCompany.UpdateDate = dto.UpdateDate;
            existingCompany.DeleteDate = dto.DeleteDate;

            await _commandRepository.UpdateAsync(existingCompany, cancellationToken);

            _logger.LogInformation("Company with id {Id} updated successfully", dto.Id);

            return dto;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete company with empty id");
                throw new WrongIdException(typeof(Company));
            }

            await _commandRepository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Company with id {Id} deleted", id);
        }
    }
}
﻿using Microsoft.Extensions.Logging;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Security;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Company;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

using Task = System.Threading.Tasks.Task;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing Company entities.
    /// Provides CRUD operations and uses logging for diagnostics and monitoring.
    /// </summary>
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IAccessService _accessService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CompanyService> _logger;

        private static readonly TimeSpan CompanyCacheExpiry = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyService"/> class.
        /// </summary>
        /// <param name="commandRepository">The repository for performing auditable company commands (e.g., insert, update).</param>
        /// <param name="queryRepository">The repository for performing auditable company queries (e.g., get by id).</param>
        /// <param name="logger">The logger for logging company service events.</param>
        public CompanyService(
            ICompanyRepository companyRepository,
            IAccessService accessService,
            ICacheService cacheService,
            ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<CompanyDto> CreateAsync(
            CompanyCreateDto createDto,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            _logger.LogInformation("User {UserId} is creating new company: {Name}", userId, createDto.Name);

            var newCompany = createDto.ToCompany();
            newCompany.Id = Guid.NewGuid();

            await _companyRepository.InsertAsync(newCompany, cancellationToken);

            var createdCompany = await _companyRepository.GetByIdAsync(newCompany.Id, cancellationToken);
            if (createdCompany == null)
                throw new NotFoundException(typeof(Company));

            var companyDto = createdCompany.ToCompanyDto();
            await _cacheService.SetAsync(CacheKeys.Company(newCompany.Id), companyDto, CompanyCacheExpiry);

            _logger.LogInformation("Company created successfully with id: {Id} by user {UserId}", newCompany.Id, userId);

            return companyDto;
        }

        /// <inheritdoc/>
        public async Task<CompanyDto> GetByIdAsync(
            Guid id,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get company with empty id");
                throw new WrongIdException(typeof(Company));
            }

            if (!await _accessService.HasPermissionAsync(userId, id, ResourceType.Company, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to view company {CompanyId}", userId, id);
                throw new ForbiddenException();
            }

            var cacheKey = CacheKeys.Company(id);
            var cachedCompany = await _cacheService.GetAsync<CompanyDto>(cacheKey);
            if (cachedCompany != null)
            {
                _logger.LogDebug("Company with id {Id} found in cache", id);
                return cachedCompany;
            }

            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);
            if (company == null)
            {
                _logger.LogWarning("Company with id {Id} not found", id);
                return null;
            }

            var companyDto = company.ToCompanyDto();
            await _cacheService.SetAsync(cacheKey, companyDto, CompanyCacheExpiry);

            return companyDto;
        }

        /// <inheritdoc/>
        public async Task<CompanyDto> UpdateAsync(
            CompanyDto dto,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to update company with empty id");
                throw new WrongIdException(typeof(Company));
            }

            if (!await _accessService.HasPermissionAsync(userId, dto.Id, ResourceType.Company, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to update company {CompanyId}", userId, dto.Id);
                throw new ForbiddenException();
            }

            var existingCompany = await _companyRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existingCompany == null)
            {
                _logger.LogWarning("Company with id {Id} not found for update", dto.Id);
                throw new NotFoundException(typeof(Company));
            }

            existingCompany.UpdateFromDto(dto);

            await _companyRepository.UpdateAsync(existingCompany, cancellationToken);

            var updatedDto = existingCompany.ToCompanyDto();
            await _cacheService.SetAsync(CacheKeys.Company(dto.Id), updatedDto, CompanyCacheExpiry);

            _logger.LogInformation("Company with id {Id} updated successfully by user {UserId}", dto.Id, userId);

            return updatedDto;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(
            Guid id,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete company with empty id");
                throw new WrongIdException(typeof(Company));
            }

            if (!await _accessService.HasPermissionAsync(userId, id, ResourceType.Company, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to delete company {CompanyId}", userId, id);
                throw new ForbiddenException();
            }

            await _companyRepository.DeleteAsync(id, cancellationToken);

            await _cacheService.RemoveAsync(CacheKeys.Company(id));

            _logger.LogInformation("Company with id {Id} deleted by user {UserId}", id, userId);
        }
    }
}
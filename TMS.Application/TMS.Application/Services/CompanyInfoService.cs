using Microsoft.Extensions.Logging;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Security;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Board;
using TMS.Application.Dto.Company;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;

using Task = System.Threading.Tasks.Task;

namespace TMS.Application.Services
{
    public class CompanyInfoService : ICompanyInfoService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IAccessService _accessService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CompanyService> _logger;

        private static readonly TimeSpan CompanyInfoCacheExpiry = TimeSpan.FromMinutes(10);

        public CompanyInfoService(
            ICompanyRepository companyRepository,
            IBoardRepository boardRepository,
            IAccessService accessService,
            ICacheService cacheService,
            ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CompanyInfoDto?> GetCompanyInfoByUserId(Guid userId, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"company-info:{userId}";
            var cached = await _cacheService.GetAsync<CompanyInfoDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogDebug("Company info for user {UserId} found in cache.", userId);
                return cached;
            }

            var company = await _companyRepository.GetCompanyByUserIdAsync(userId, cancellationToken);

            if (company == null)
            {
                _logger.LogWarning("No company found for user {UserId}", userId);
                return null;
            }

            var companyBoards = await _boardRepository.GetBoardsByCompanyIdAsync(company.Id, cancellationToken);
            var userBoards = companyBoards.Where(b => b.HeadId == userId || b.Users.Any(u => u.Id == userId)).ToList();

            var boardsDto = userBoards.Select(b => new BoardInfoDto
            {
                Id = b.Id,
                Name = b.Name,
                OwnerFullName = b.Head?.FullName ?? "—",
                IsPrivate = b.IsPrivate
            }).ToList();

            var dto = new CompanyInfoDto
            {
                UserId = company.Id,
                Name = company.Name,
                Logo = company.Logo,
                INN = company.INN,
                OGRN = company.OGRN,
                Address = company.Address,
                Website = company.Website,
                Industry = company.Industry,
                Description = company.Description,
                ContactEmail = company.ContactEmail,
                ContactPhone = company.ContactPhone,
                IsActive = company.DeleteDate == null,
                Boards = boardsDto
            };

            await _cacheService.SetAsync(cacheKey, dto, CompanyInfoCacheExpiry);
            return dto;
        }

        public Task GetTarifInfoByCompanyId(Guid companyId, Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task GetCEOInfoByCompanyId(Guid companyId, Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
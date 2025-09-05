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
using TMS.Infrastructure.DataModels;
using Task = System.Threading.Tasks.Task;

namespace TMS.Application.Services
{
    public class CompanyInfoService : ICompanyInfoService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IColumnRepository _columnRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IAccessService _accessService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CompanyService> _logger;

        private static readonly TimeSpan CompanyInfoCacheExpiry = TimeSpan.FromMinutes(10);

        public CompanyInfoService(
            IUserRepository userRepository,
            ITaskRepository taskRepository,
            IColumnRepository columnRepository,
            ICompanyRepository companyRepository,
            IBoardRepository boardRepository,
            IAccessService accessService,
            ICacheService cacheService,
            ILogger<CompanyService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _columnRepository = columnRepository ?? throw new ArgumentNullException(nameof(columnRepository));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CompanyInfoDto?> GetCompanyInfoByUserId(
            Guid userId,
            CancellationToken cancellationToken = default)
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
            var userBoards = companyBoards.Where(b => b.HeadId == userId || b.BoardUsers.Any(u => u.UsersId == userId)).ToList();

            var boardsDto = userBoards.Select(
                    b => new BoardInfoDto
                    {
                        Id = b.Id,
                        Name = b.Name,
                        OwnerFullName = b.Head?.FullName ?? "—",
                        IsPrivate = b.IsPrivate
                    })
                .ToList();

            var dto = new CompanyInfoDto
            {
                Id = company.Id,
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

        public async Task<CEOSummaryDto?> GetCEOInfoByCompanyId(
            Guid companyId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var cacheKey = $"company-info:{userId}";
            var cached = await _cacheService.GetAsync<CEOSummaryDto>(cacheKey);

            if (cached != null)
            {
                _logger.LogDebug("CEO summary info for user {UserId} found in cache.", userId);
                return cached;
            }

            var companyBoards = await _boardRepository.GetBoardsByCompanyIdAsync(companyId, cancellationToken);
            List<Column> columns = new List<Column>();

            foreach (var companyBoard in companyBoards)
                columns = await _columnRepository.GetColumnsByBoardIdAsync(companyBoard.Id, cancellationToken);

            var tasks = await _taskRepository.GetTasksByColumnIdsAsync(columns.Select(c => c.Id), cancellationToken);

            var totalTasks = tasks.Count();
            var completedTasks = tasks.Count(task => task.IsCompletedTask);
            var tasksInWork = tasks.Count(t => t.Assignee != null);

            var groupBoards = tasks
                .GroupBy(b => b.BoardId)
                .OrderBy(b => b);

            var leaderBoardId = groupBoards.Max(b => b.Key);
            var leaderBoard = await _boardRepository.GetByIdAsync(leaderBoardId, cancellationToken);
            
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            return new CEOSummaryDto
            {
                TotalTasks = totalTasks,
                TotalDone = completedTasks,
                TotalInProgress = tasksInWork,
                LeadBoard = leaderBoard.Name,
                MostActiveUser = user.FullName ?? "Данные не указаны"
            };
        }
    }
}
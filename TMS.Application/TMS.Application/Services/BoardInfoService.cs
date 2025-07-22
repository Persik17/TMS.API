using Microsoft.Extensions.Logging;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Board;
using TMS.Application.Dto.Column;
using TMS.Application.Dto.Task;
using TMS.Application.Dto.TaskType;
using TMS.Infrastructure.Abstractions.Repositories;

namespace TMS.Application.Services
{
    public class BoardInfoService : IBoardInfoService
    {
        private readonly IColumnRepository _columnRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskTypeRepository _taskTypeRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<BoardInfoService> _logger;
        private static readonly TimeSpan BoardInfoCacheExpiry = TimeSpan.FromMinutes(10);

        public BoardInfoService(
            IColumnRepository columnRepository,
            ITaskRepository taskRepository,
            ITaskTypeRepository taskTypeRepository,
            ICacheService cacheService,
            ILogger<BoardInfoService> logger)
        {
            _columnRepository = columnRepository ?? throw new ArgumentNullException(nameof(columnRepository));
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _taskTypeRepository = taskTypeRepository ?? throw new ArgumentNullException(nameof(taskTypeRepository));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BoardSummaryInfoDto?> GetBoardInfoAsync(Guid boardId, Guid userId, Guid companyId, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"board-info:{boardId}";
            var cached = await _cacheService.GetAsync<BoardSummaryInfoDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogDebug("Board info for {BoardId} found in cache.", boardId);
                return cached;
            }

            // Колонки
            var columns = await _columnRepository.GetColumnsByBoardIdAsync(boardId, cancellationToken);
            var columnIds = columns.Select(c => c.Id).ToList();

            // Таски по всем колонкам одним запросом
            var tasks = await _taskRepository.GetTasksByColumnIdsAsync(columnIds, cancellationToken);

            var columnsDto = columns.Select(col => new BoardColumnInfoDto
            {
                Id = col.Id,
                Title = col.Name,
                Color = col.Color,
                Order = col.Order,
                Tasks = tasks
                    .Where(t => t.ColumnId == col.Id)
                    .Select(t => new BoardTaskInfoDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        AssigneeId = t.AssigneeId,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate,
                        StoryPoints = t.StoryPoints,
                        Priority = t.Priority,
                        TaskTypeId = t.TaskTypeId
                    }).ToList()
            }).ToList();

            var taskTypes = await _taskTypeRepository.GetTaskTypesByBoardIdAsync(boardId, cancellationToken);
            var taskTypesDto = taskTypes.Select(tt => new BoardTaskTypeDto
            {
                Id = tt.Id,
                Name = tt.Name,
            }).ToList();

            var dto = new BoardSummaryInfoDto
            {
                BoardId = boardId,
                Columns = columnsDto,
                TaskTypes = taskTypesDto
            };

            await _cacheService.SetAsync(cacheKey, dto, BoardInfoCacheExpiry);
            return dto;
        }
    }
}

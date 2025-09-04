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
        private readonly IUserRepository _userRepository;
        private readonly ILogger<BoardInfoService> _logger;

        public BoardInfoService(
            IColumnRepository columnRepository,
            ITaskRepository taskRepository,
            ITaskTypeRepository taskTypeRepository,
            ICacheService cacheService,
            IUserRepository userRepository,
            ILogger<BoardInfoService> logger)
        {
            _columnRepository = columnRepository ?? throw new ArgumentNullException(nameof(columnRepository));
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _taskTypeRepository = taskTypeRepository ?? throw new ArgumentNullException(nameof(taskTypeRepository));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BoardSummaryInfoDto?> GetBoardInfoAsync(Guid boardId, Guid userId, Guid companyId, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"board-info:{boardId}";

            var columns = await _columnRepository.GetColumnsByBoardIdAsync(boardId, cancellationToken);
            var columnIds = columns.Select(c => c.Id).ToList();
            var tasks = await _taskRepository.GetTasksByColumnIdsAsync(columnIds, cancellationToken);

            var assigneeIds = tasks
                .Where(t => t.AssigneeId.HasValue)
                .Select(t => t.AssigneeId.Value)
                .Distinct()
                .ToList();

            var assignees = await _userRepository.GetUsersByBoardIdAsync(boardId, cancellationToken);
            var assigneeDict = assignees
                .Where(u => assigneeIds.Contains(u.Id))
                .ToDictionary(u => u.Id, u => u.FullName);

            var columnsDto = columns.Select(col => new BoardColumnInfoDto
            {
                Id = col.Id,
                Title = col.Name,
                Color = col.Color,
                Order = col.Order,
                Tasks = [.. tasks
                    .Where(t => t.ColumnId == col.Id)
                    .Select(t => new BoardTaskInfoDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        AssigneeId = t.AssigneeId,
                        AssigneeName = t.AssigneeId.HasValue && assigneeDict.TryGetValue(t.AssigneeId.Value, out var name) ? name : null,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate,
                        StoryPoints = t.StoryPoints,
                        Priority = t.Priority,
                        TaskTypeId = t.TaskTypeId
                    })]
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

            return dto;
        }

        public async Task<BoardAnalyticsDto> GetBoardAnalyticsAsync(Guid boardId, Guid userId, CancellationToken cancellationToken = default)
        {
            var columns = await _columnRepository.GetColumnsByBoardIdAsync(boardId, cancellationToken);
            var columnIds = columns.Select(c => c.Id).ToList();
            var tasks = await _taskRepository.GetTasksByColumnIdsAsync(columnIds, cancellationToken);

            if (tasks.Count == 0)
            {
                return new BoardAnalyticsDto
                {
                    Velocity = new List<VelocityPointDto>(),
                    BurnDown = new List<BurnDownPointDto>(),
                    CFD = new List<CfdPointDto>()
                };
            }

            var velocity = tasks
                .Where(t => t.EndDate.HasValue)
                .GroupBy(t => t.EndDate.Value.ToString("yyyy-MM"))
                .Select(g => new VelocityPointDto
                {
                    SprintName = g.Key,
                    CompletedStoryPoints = g.Sum(t => t.StoryPoints ?? 0)
                })
                .OrderBy(v => v.SprintName)
                .ToList();

            var startDate = tasks.Min(t => t.CreationDate.Date);
            var endDate = tasks.Max(t => (t.EndDate ?? DateTime.UtcNow).Date);
            var allDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                .Select(offset => startDate.AddDays(offset))
                .ToList();

            var initialTasksCount = tasks.Count;
            var burnDown = new List<BurnDownPointDto>();
            foreach (var date in allDates)
            {
                var remaining = tasks.Count(t => !t.EndDate.HasValue || t.EndDate.Value.Date > date);
                var ideal = Math.Max(0, initialTasksCount - (initialTasksCount * (date - startDate).Days / allDates.Count));
                burnDown.Add(new BurnDownPointDto
                {
                    Date = date,
                    RemainingTasks = remaining,
                    IdealTasks = ideal
                });
            }

            var cfd = allDates.Select(day => new CfdPointDto
            {
                Date = day,
                ColumnTaskCounts = columns.ToDictionary(
                    col => col.Name,
                    col => tasks.Count(t =>
                        t.CreationDate <= day &&
                        (!t.EndDate.HasValue || t.EndDate > day) &&
                        t.ColumnId == col.Id)
                )
            }).ToList();

            return new BoardAnalyticsDto
            {
                Velocity = velocity,
                BurnDown = burnDown,
                CFD = cfd
            };
        }
    }
}
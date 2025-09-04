using Microsoft.Extensions.Logging;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Factories;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Board;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Provides a service for managing boards.
    /// Implements operations for creating, retrieving, updating and deleting boards.
    /// </summary>
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IAccessService _accessService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<BoardService> _logger;
        private readonly IColumnRepository _columnRepository;
        private readonly ITaskTypeRepository _taskTypeRepository;
        private readonly IColumnFactory _columnFactory;

        private static readonly TimeSpan BoardCacheExpiry = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardService"/> class.
        /// </summary>
        /// <param name="boardRepository">The repository for accessing board data.</param>
        /// <param name="accessService">The service for permission checks.</param>
        /// <param name="cacheService">Service for caching board data.</param>
        /// <param name="logger">The logger for logging board service events.</param>
        public BoardService(
            IBoardRepository boardRepository,
            IAccessService accessService,
            ICacheService cacheService,
            ILogger<BoardService> logger,
            IColumnRepository columnRepository,
            ITaskTypeRepository taskTypeRepository,
            IColumnFactory columnFactory)
        {
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _columnRepository = columnRepository ?? throw new ArgumentNullException(nameof(columnRepository));
            _taskTypeRepository = taskTypeRepository ?? throw new ArgumentNullException(nameof(taskTypeRepository));
            _columnFactory = columnFactory ?? throw new ArgumentNullException(nameof(columnFactory));
        }

        /// <inheritdoc/>
        public async Task<BoardDto> CreateAsync(BoardCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            _logger.LogInformation("User {UserId} is creating new board: {Name}", userId, createDto.Name);

            var newBoard = createDto.ToBoard();
            newBoard.Id = Guid.NewGuid();
            newBoard.HeadId = userId;

            await _boardRepository.InsertAsync(newBoard, cancellationToken);

            var columns = _columnFactory.CreateDefaultColumns(newBoard.Id);
            foreach (var column in columns)
            {
                await _columnRepository.InsertAsync(column, cancellationToken);
            }

            var defaultTaskTypes = new[]
            {
                new TaskType
                {
                    Id = Guid.NewGuid(),
                    BoardId = newBoard.Id,
                    Name = "Задача",
                    Description = "Обычная задача",
                },
                new TaskType
                {
                    Id = Guid.NewGuid(),
                    BoardId = newBoard.Id,
                    Name = "Юзерстори",
                    Description = "User Story",
                },
                new TaskType
                {
                    Id = Guid.NewGuid(),
                    BoardId = newBoard.Id,
                    Name = "Эпик",
                    Description = "Epic",
                }
            };

            foreach (var taskType in defaultTaskTypes)
            {
                await _taskTypeRepository.InsertAsync(taskType, cancellationToken);
            }

            var createdBoard = await _boardRepository.GetByIdAsync(newBoard.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Board));

            var boardDto = createdBoard.ToBoardDto();
            await _cacheService.SetAsync(CacheKeys.Board(newBoard.Id), boardDto, BoardCacheExpiry);
            await _cacheService.RemoveAsync(CacheKeys.BoardsByCompany(newBoard.CompanyId));

            _logger.LogInformation("Board created successfully with id: {Id} by user {UserId}", newBoard.Id, userId);

            return boardDto;
        }


        /// <inheritdoc/>
        public async Task<BoardDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get board with empty id");
                throw new WrongIdException(typeof(Board));
            }

            if (!await _accessService.HasPermissionAsync(userId, id, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to view board {BoardId}", userId, id);
                throw new ForbiddenException();
            }

            var cacheKey = CacheKeys.Board(id);
            var cachedBoard = await _cacheService.GetAsync<BoardDto>(cacheKey);
            if (cachedBoard != null)
            {
                _logger.LogDebug("Board with id {Id} found in cache", id);
                return cachedBoard;
            }

            var board = await _boardRepository.GetByIdAsync(id, cancellationToken);
            if (board == null)
            {
                _logger.LogWarning("Board with id {Id} not found", id);
                return null;
            }

            var boardDto = board.ToBoardDto();
            await _cacheService.SetAsync(cacheKey, boardDto, BoardCacheExpiry);

            return boardDto;
        }

        /// <inheritdoc/>
        public async Task<BoardDto> UpdateAsync(BoardDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to update board with empty id");
                throw new WrongIdException(typeof(Board));
            }

            if (!await _accessService.HasPermissionAsync(userId, dto.Id, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to update board {BoardId}", userId, dto.Id);
                throw new ForbiddenException();
            }

            var existingBoard = await _boardRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existingBoard == null)
            {
                _logger.LogWarning("Board with id {Id} not found for update", dto.Id);
                throw new NotFoundException(typeof(Board));
            }

            existingBoard.Name = dto.Name;
            existingBoard.Description = dto.Description;
            existingBoard.CompanyId = dto.CompanyId;
            existingBoard.IsPrivate = dto.IsPrivate;
            existingBoard.UpdateDate = DateTime.UtcNow;

            await _boardRepository.UpdateAsync(existingBoard, cancellationToken);

            var updatedDto = existingBoard.ToBoardDto();
            await _cacheService.SetAsync(CacheKeys.Board(dto.Id), updatedDto, BoardCacheExpiry);

            _logger.LogInformation("Board with id {Id} updated successfully by user {UserId}", dto.Id, userId);

            return updatedDto;
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete board with empty id");
                throw new WrongIdException(typeof(Board));
            }

            if (!await _accessService.HasPermissionAsync(userId, id, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to delete board {BoardId}", userId, id);
                throw new ForbiddenException();
            }

            await _boardRepository.DeleteAsync(id, cancellationToken);

            await _cacheService.RemoveAsync(CacheKeys.Board(id));

            _logger.LogInformation("Board with id {Id} deleted by user {UserId}", id, userId);
        }

        public async Task<List<BoardDto>> GetBoardsByCompanyIdAsync(Guid companyId, Guid userId, CancellationToken cancellationToken = default)
        {
            if (companyId == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get boards with empty department id");
                throw new WrongIdException(typeof(Board));
            }

            if (!await _accessService.HasPermissionAsync(userId, companyId, ResourceType.Company, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to view boards in department {DepartmentId}", userId, companyId);
                throw new ForbiddenException();
            }

            var cacheKey = CacheKeys.BoardsByCompany(companyId);
            var cachedBoards = await _cacheService.GetAsync<List<BoardDto>>(cacheKey);
            if (cachedBoards != null)
            {
                _logger.LogDebug("Boards for department {DepartmentId} found in cache", companyId);
                return cachedBoards;
            }

            var boards = await _boardRepository.GetBoardsByCompanyIdAsync(companyId, cancellationToken) ?? [];
            var boardDtos = new List<BoardDto>();
            foreach (var board in boards)
            {
                boardDtos.Add(board.ToBoardDto());
            }

            await _cacheService.SetAsync(cacheKey, boardDtos, BoardCacheExpiry);

            return boardDtos;
        }
    }
}
using Microsoft.Extensions.Logging;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Application.Abstractions.Cache;
using TMS.Application.Abstractions.Security;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Column;
using TMS.Application.Extensions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Provides a service for managing columns.
    /// Implements operations for creating, retrieving, and updating columns.
    /// </summary>
    public class ColumnService : IColumnService
    {
        private readonly IColumnRepository _columnRepository;
        private readonly IAccessService _accessService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ColumnService> _logger;

        private static readonly TimeSpan ColumnCacheExpiry = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnService"/> class.
        /// </summary>
        /// <param name="columnRepository">The repository for performing column operations.</param>
        /// <param name="accessService">The access service for permission checks.</param>
        /// <param name="cacheService">The cache service for caching column data.</param>
        /// <param name="logger">The logger for logging column service events.</param>
        public ColumnService(
            IColumnRepository columnRepository,
            IAccessService accessService,
            ICacheService cacheService,
            ILogger<ColumnService> logger)
        {
            _columnRepository = columnRepository ?? throw new ArgumentNullException(nameof(columnRepository));
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<ColumnDto> CreateAsync(ColumnCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            _logger.LogInformation("User {UserId} is creating new column: {Name}", userId, createDto.Name);

            if (!await _accessService.HasPermissionAsync(userId, Guid.Empty, ResourceType.Column, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to create column in board {BoardId}", userId, Guid.Empty);
                throw new ForbiddenException();
            }

            var newColumn = createDto.ToColumn();
            newColumn.Id = Guid.NewGuid();

            await _columnRepository.InsertAsync(newColumn, cancellationToken);

            var createdColumn = await _columnRepository.GetByIdAsync(newColumn.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Column));

            var columnDto = createdColumn.ToColumnDto();
            await _cacheService.SetAsync(CacheKeys.Column(newColumn.Id), columnDto, ColumnCacheExpiry);

            _logger.LogInformation("Column created successfully with id: {Id} by user {UserId}", newColumn.Id, userId);

            return columnDto;
        }

        /// <inheritdoc/>
        public async Task<ColumnDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get column with empty id");
                throw new WrongIdException(typeof(Column));
            }

            if (!await _accessService.HasPermissionAsync(userId, id, ResourceType.Column, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to view column {ColumnId}", userId, id);
                throw new ForbiddenException();
            }

            var cacheKey = CacheKeys.Column(id);
            var cachedColumn = await _cacheService.GetAsync<ColumnDto>(cacheKey);
            if (cachedColumn != null)
            {
                _logger.LogDebug("Column with id {Id} found in cache", id);
                return cachedColumn;
            }

            var column = await _columnRepository.GetByIdAsync(id, cancellationToken);
            if (column == null)
            {
                _logger.LogWarning("Column with id {Id} not found", id);
                return null;
            }

            var columnDto = column.ToColumnDto();
            await _cacheService.SetAsync(cacheKey, columnDto, ColumnCacheExpiry);

            return columnDto;
        }

        /// <inheritdoc/>
        public async Task<ColumnDto> UpdateAsync(ColumnDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to update column with empty id");
                throw new WrongIdException(typeof(Column));
            }

            if (!await _accessService.HasPermissionAsync(userId, dto.Id, ResourceType.Column, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to update column {ColumnId}", userId, dto.Id);
                throw new ForbiddenException();
            }

            var existingColumn = await _columnRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existingColumn == null)
            {
                _logger.LogWarning("Column with id {Id} not found for update", dto.Id);
                throw new NotFoundException(typeof(Column));
            }

            existingColumn.Name = dto.Name;
            existingColumn.Description = dto.Description;
            existingColumn.Order = dto.Order;
            existingColumn.Color = dto.Color;
            existingColumn.UpdateDate = DateTime.UtcNow;

            await _columnRepository.UpdateAsync(existingColumn, cancellationToken);

            var updatedDto = existingColumn.ToColumnDto();
            await _cacheService.SetAsync(CacheKeys.Column(dto.Id), updatedDto, ColumnCacheExpiry);

            _logger.LogInformation("Column with id {Id} updated successfully by user {UserId}", dto.Id, userId);

            return updatedDto;
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete column with empty id");
                throw new WrongIdException(typeof(Column));
            }

            if (!await _accessService.HasPermissionAsync(userId, id, ResourceType.Column, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to delete column {ColumnId}", userId, id);
                throw new ForbiddenException();
            }

            await _columnRepository.DeleteAsync(id, cancellationToken);

            await _cacheService.RemoveAsync(CacheKeys.Column(id));

            _logger.LogInformation("Column with id {Id} deleted by user {UserId}", id, userId);
        }

        public async Task<List<ColumnDto>> GetColumnsByBoardIdAsync(Guid boardId, Guid userId, CancellationToken cancellationToken = default)
        {
            if (boardId == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get columns with empty board id");
                throw new WrongIdException(typeof(Column));
            }

            if (!await _accessService.HasPermissionAsync(userId, boardId, ResourceType.Board, cancellationToken))
            {
                _logger.LogWarning("User {UserId} has no permission to view columns in board {BoardId}", userId, boardId);
                throw new ForbiddenException();
            }

            var cacheKey = CacheKeys.ColumnsByBoard(boardId);
            var cachedColumns = await _cacheService.GetAsync<List<ColumnDto>>(cacheKey);
            if (cachedColumns != null)
            {
                _logger.LogDebug("Columns for board {BoardId} found in cache", boardId);
                return cachedColumns;
            }

            var columns = await _columnRepository.GetColumnsByBoardIdAsync(boardId, cancellationToken) ?? new List<Column>();
            var columnDtos = new List<ColumnDto>();
            foreach (var column in columns)
            {
                columnDtos.Add(column.ToColumnDto());
            }

            await _cacheService.SetAsync(cacheKey, columnDtos, ColumnCacheExpiry);

            return columnDtos;
        }
    }
}
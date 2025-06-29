using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
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
        private readonly ILogger<BoardService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardService"/> class.
        /// </summary>
        /// <param name="boardRepository">The repository for accessing board data.</param>
        /// <param name="logger">The logger for logging board service events.</param>
        public BoardService(
            IBoardRepository boardRepository,
            ILogger<BoardService> logger)
        {
            _boardRepository = boardRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<BoardDto> CreateAsync(BoardCreateDto createDto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            _logger.LogInformation("Creating new board: {Name}", createDto.Name);

            var newBoard = createDto.ToBoard();
            await _boardRepository.InsertAsync(newBoard, cancellationToken);

            var createdBoard = await _boardRepository.GetByIdAsync(newBoard.Id, cancellationToken)
                ?? throw new NotFoundException(typeof(Board));

            _logger.LogInformation("Board created successfully with id: {Id}", newBoard.Id);

            return createdBoard.ToBoardDto();
        }

        /// <inheritdoc/>
        public async Task<BoardDto> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get board with empty id");
                throw new WrongIdException(typeof(Board));
            }

            var board = await _boardRepository.GetByIdAsync(id, cancellationToken);
            if (board == null)
            {
                _logger.LogWarning("Board with id {Id} not found", id);
                return null; // Consider returning null instead of throwing an exception if the board is not found
            }

            return board.ToBoardDto(); // Use non-nullable assertion
        }

        /// <inheritdoc/>
        public async Task<BoardDto> UpdateAsync(BoardDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.Id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to update board with empty id");
                throw new WrongIdException(typeof(Board));
            }

            var existingBoard = await _boardRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existingBoard == null)
            {
                _logger.LogWarning("Board with id {Id} not found for update", dto.Id);
                throw new NotFoundException(typeof(Board));
            }

            existingBoard.Name = dto.Name;
            existingBoard.Description = dto.Description;
            existingBoard.DepartmentId = dto.DepartmentId;
            existingBoard.IsPrivate = dto.IsPrivate;
            existingBoard.UpdateDate = DateTime.UtcNow;

            await _boardRepository.UpdateAsync(existingBoard, cancellationToken);

            _logger.LogInformation("Board with id {Id} updated successfully", dto.Id);

            return dto;
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete board with empty id");
                throw new WrongIdException(typeof(Board));
            }

            await _boardRepository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Board with id {Id} deleted", id);
        }
    }
}
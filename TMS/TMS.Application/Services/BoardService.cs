using Microsoft.Extensions.Logging;
using TMS.Abstractions.Exceptions;
using TMS.Abstractions.Interfaces.Repositories;
using TMS.Abstractions.Interfaces.Services;
using TMS.Application.DTOs.Board;
using TMS.Application.Extensions;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Services
{
    /// <summary>
    /// Service for managing Board entities.
    /// Provides CRUD operations and uses logging for diagnostics and monitoring.
    /// </summary>
    public class BoardService : IBoardService<BoardDto, BoardCreateDto>
    {
        private readonly IBoardRepository<Board> _boardRepository;
        private readonly ILogger<BoardService> _logger;

        public BoardService(
            IBoardRepository<Board> boardRepository,
            ILogger<BoardService> logger)
        {
            _boardRepository = boardRepository;
            _logger = logger;
        }

        public async Task<BoardDto> CreateAsync(BoardCreateDto createDto, CancellationToken cancellationToken = default)
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

        public async Task<BoardDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to get board with empty id");
                throw new WrongIdException(typeof(Board));
            }

            var board = await _boardRepository.GetByIdAsync(id, cancellationToken);
            if (board == null)
                _logger.LogWarning("Board with id {Id} not found", id);

            return board?.ToBoardDto();
        }

        public async Task<BoardDto> UpdateAsync(BoardDto dto, CancellationToken cancellationToken = default)
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
            existingBoard.BoardType = dto.BoardType;
            existingBoard.IsPrivate = dto.IsPrivate;
            existingBoard.UpdateDate = DateTime.UtcNow;

            await _boardRepository.UpdateAsync(existingBoard, cancellationToken);

            _logger.LogInformation("Board with id {Id} updated successfully", dto.Id);

            return dto;
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
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
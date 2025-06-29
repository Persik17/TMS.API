using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.API.ViewModels.Board;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Board;
using TMS.Infrastructure.DataModels;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for managing boards.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly ILogger<BoardController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardController"/> class.
        /// </summary>
        /// <param name="boardService">The board service.</param>
        /// <param name="logger">The logger.</param>
        public BoardController(
            IBoardService boardService,
            ILogger<BoardController> logger)
        {
            _boardService = boardService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a board by its ID.
        /// </summary>
        /// <param name="id">The ID of the board to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> representing the retrieved board or a 404 Not Found if the board does not exist.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BoardViewModel>> GetById(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var board = await _boardService.GetByIdAsync(id, userId, cancellationToken);
            if (board == null)
            {
                _logger.LogWarning("Board with id {Id} not found", id);
                return NotFound();
            }

            var viewModel = new BoardViewModel
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                DepartmentId = board.DepartmentId,
                BoardType = board.BoardType,
                IsPrivate = board.IsPrivate,
                CreationDate = board.CreationDate,
                UpdateDate = board.UpdateDate,
                DeleteDate = board.DeleteDate
            };

            return Ok(viewModel);
        }

        /// <summary>
        /// Creates a new board.
        /// </summary>
        /// <param name="request">The request containing the data for the new board.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> representing the newly created board.</returns>
        [HttpPost]
        public async Task<ActionResult<BoardViewModel>> Create([FromBody] BoardViewModel request, Guid userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Create called with null model");
                return BadRequest("Board data is required.");
            }

            var dto = new BoardCreateDto
            {
                Name = request.Name,
                Description = request.Description,
                DepartmentId = request.DepartmentId,
                BoardType = request.BoardType,
                IsPrivate = request.IsPrivate
            };

            var board = await _boardService.CreateAsync(dto, userId, cancellationToken);

            var viewModel = new BoardViewModel
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                DepartmentId = board.DepartmentId,
                BoardType = board.BoardType,
                IsPrivate = board.IsPrivate,
                CreationDate = board.CreationDate,
                UpdateDate = board.UpdateDate,
                DeleteDate = board.DeleteDate
            };

            return CreatedAtAction(nameof(GetById), new { id = board.Id }, viewModel);
        }

        /// <summary>
        /// Updates an existing board.
        /// </summary>
        /// <param name="id">The ID of the board to update.</param>
        /// <param name="request">The request containing the updated data for the board.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BoardViewModel request, Guid userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Update called with null model");
                return BadRequest("Board data is required.");
            }
            if (id != request.Id)
            {
                _logger.LogWarning("Update id mismatch: route id {RouteId}, body id {BodyId}", id, request.Id);
                return BadRequest("ID mismatch");
            }

            var dto = new BoardDto
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                DepartmentId = request.DepartmentId,
                BoardType = request.BoardType,
                IsPrivate = request.IsPrivate,
                CreationDate = request.CreationDate,
                UpdateDate = request.UpdateDate,
                DeleteDate = request.DeleteDate
            };

            await _boardService.UpdateAsync(dto, userId, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes a board.
        /// </summary>
        /// <param name="id">The ID of the board to delete.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the delete operation.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            //NOTE: Add check for valid id before deletion.
            await _boardService.DeleteAsync(id, userId, cancellationToken);
            return NoContent();
        }
    }
}
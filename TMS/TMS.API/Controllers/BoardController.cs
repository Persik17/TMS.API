using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.API.ViewModels.Board;
using TMS.Application.DTOs.Board;

namespace TMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService<BoardDto, BoardCreateDto> _boardService;
        private readonly ILogger<BoardController> _logger;

        public BoardController(
            IBoardService<BoardDto, BoardCreateDto> boardService,
            ILogger<BoardController> logger)
        {
            _boardService = boardService;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BoardViewModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var board = await _boardService.GetByIdAsync(id, cancellationToken);
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

        [HttpPost]
        public async Task<ActionResult<BoardViewModel>> Create([FromBody] BoardCreateViewModel model, CancellationToken cancellationToken)
        {
            if (model == null)
            {
                _logger.LogWarning("Create called with null model");
                return BadRequest("Board data is required.");
            }

            var dto = new BoardCreateDto
            {
                Name = model.Name,
                Description = model.Description,
                DepartmentId = model.DepartmentId,
                BoardType = model.BoardType,
                IsPrivate = model.IsPrivate
            };

            var board = await _boardService.CreateAsync(dto, cancellationToken);

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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BoardViewModel model, CancellationToken cancellationToken)
        {
            if (model == null)
            {
                _logger.LogWarning("Update called with null model");
                return BadRequest("Board data is required.");
            }
            if (id != model.Id)
            {
                _logger.LogWarning("Update id mismatch: route id {RouteId}, body id {BodyId}", id, model.Id);
                return BadRequest("ID mismatch");
            }

            var dto = new BoardDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                DepartmentId = model.DepartmentId,
                BoardType = model.BoardType,
                IsPrivate = model.IsPrivate,
                CreationDate = model.CreationDate,
                UpdateDate = model.UpdateDate,
                DeleteDate = model.DeleteDate
            };

            await _boardService.UpdateAsync(dto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _boardService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.API.ViewModels.Board;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto;
using TMS.Application.Dto.Board;
using TMS.Application.Dto.Column;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for managing boards.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/companies/{companyId}/boards")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly IBoardInfoService _boardInfoService;
        private readonly IColumnService _columnService;
        private readonly ILogger<BoardController> _logger;

        public BoardController(
            IBoardService boardService,
            IBoardInfoService boardInfoService,
            IColumnService columnService,
            ILogger<BoardController> logger)
        {
            _boardService = boardService;
            _boardInfoService = boardInfoService;
            _columnService = columnService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a board by its ID (basic info).
        /// </summary>
        [HttpGet("{boardId:guid}")]
        public async Task<ActionResult<BoardViewModel>> GetById(
            Guid companyId,
            Guid boardId,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var board = await _boardService.GetByIdAsync(boardId, userId, cancellationToken);
            if (board == null)
            {
                _logger.LogWarning("Board with id {Id} not found", boardId);
                return NotFound();
            }

            var viewModel = new BoardViewModel
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                CompanyId = board.CompanyId,
                BoardType = board.BoardType,
                IsPrivate = board.IsPrivate,
                CreationDate = board.CreationDate,
                UpdateDate = board.UpdateDate,
                DeleteDate = board.DeleteDate
            };

            return Ok(viewModel);
        }

        /// <summary>
        /// Retrieves board info with columns, tasks and task types.
        /// </summary>
        [HttpGet("{boardId:guid}/info")]
        public async Task<ActionResult<BoardSummaryInfoDto>> GetBoardInfo(
            Guid companyId,
            Guid boardId,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var boardInfo = await _boardInfoService.GetBoardInfoAsync(boardId, userId, companyId, cancellationToken);
            if (boardInfo == null)
            {
                _logger.LogWarning("Board info for id {Id} not found", boardId);
                return NotFound();
            }
            return Ok(boardInfo);
        }

        /// <summary>
        /// Creates a new board.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BoardViewModel>> Create(
            Guid companyId,
            [FromBody] BoardViewModel request,
            Guid userId,
            CancellationToken cancellationToken)
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
                CompanyId = request.CompanyId,
                BoardType = request.BoardType,
                IsPrivate = request.IsPrivate
            };

            var board = await _boardService.CreateAsync(dto, userId, cancellationToken);

            var viewModel = new BoardViewModel
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                CompanyId = board.CompanyId,
                BoardType = board.BoardType,
                IsPrivate = board.IsPrivate,
                CreationDate = board.CreationDate,
                UpdateDate = board.UpdateDate,
                DeleteDate = board.DeleteDate
            };

            return Ok(viewModel);
        }

        /// <summary>
        /// Updates an existing board.
        /// </summary>
        [HttpPut("{boardId:guid}")]
        public async Task<IActionResult> Update(
            Guid companyId,
            Guid boardId,
            [FromBody] BoardViewModel request,
            Guid userId,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Update called with null model");
                return BadRequest("Board data is required.");
            }
            if (boardId != request.Id)
            {
                _logger.LogWarning("Update id mismatch: route id {RouteId}, body id {BodyId}", boardId, request.Id);
                return BadRequest("ID mismatch");
            }

            var dto = new BoardDto
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                CompanyId = request.CompanyId,
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
        [HttpDelete("{boardId:guid}")]
        public async Task<IActionResult> Delete(
            Guid companyId,
            Guid boardId,
            Guid userId,
            CancellationToken cancellationToken)
        {
            await _boardService.DeleteAsync(boardId, userId, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Retrieves all boards for a company.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<BoardViewModel>>> GetBoards(
            [FromRoute] Guid companyId,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken)
        {
            try
            {
                var boards = await _boardService.GetBoardsByCompanyIdAsync(companyId, userId, cancellationToken);
                var viewModels = boards.Select(board => new BoardViewModel
                {
                    Id = board.Id,
                    Name = board.Name,
                    Description = board.Description,
                    CompanyId = board.CompanyId,
                    HeadFullName = board.HeadFullName,
                    BoardType = board.BoardType,
                    IsPrivate = board.IsPrivate,
                    CreationDate = board.CreationDate,
                    UpdateDate = board.UpdateDate,
                    DeleteDate = board.DeleteDate
                }).ToList();

                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBoards");
                return StatusCode(500, ex.ToString());
            }
        }

        /// <summary>
        /// Creates a new column for the board.
        /// </summary>
        [HttpPost("{boardId:guid}/columns")]
        public async Task<ActionResult<ColumnDto>> CreateColumn(
            [FromRoute] Guid companyId,
            [FromRoute] Guid boardId,
            [FromBody] ColumnCreateDto request,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("CreateColumn called with null model");
                return BadRequest("Column data is required.");
            }

            request.BoardId = boardId;

            var columnDto = await _columnService.CreateAsync(request, userId, cancellationToken);
            return Ok(columnDto);
        }

        [HttpPut("{boardId:guid}/columns/{columnId:guid}")]
        public async Task<ActionResult<ColumnDto>> UpdateColumn(
            [FromRoute] Guid companyId,
            [FromRoute] Guid boardId,
            [FromRoute] Guid columnId,
            [FromBody] ColumnDto columnDto,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken)
        {
            if (columnDto == null)
                return BadRequest("Column data is required.");
            if (columnId != columnDto.Id)
                return BadRequest("ID mismatch");
            columnDto.BoardId = boardId;

            var updated = await _columnService.UpdateAsync(columnDto, userId, cancellationToken);
            return Ok(updated);
        }

        [HttpDelete("{boardId:guid}/columns/{columnId:guid}")]
        public async Task<IActionResult> DeleteColumn(
            [FromRoute] Guid companyId,
            [FromRoute] Guid boardId,
            [FromRoute] Guid columnId,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken)
        {
            await _columnService.DeleteAsync(columnId, userId, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{boardId:guid}/columns/order")]
        public async Task<ActionResult<List<ColumnDto>>> UpdateColumnOrder(
            [FromRoute] Guid companyId,
            [FromRoute] Guid boardId,
            [FromBody] List<ColumnDto> columns,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken)
        {
            if (columns == null)
                return BadRequest("Columns are required.");

            var updated = await _columnService.UpdateColumnOrderAsync(boardId, columns, userId, cancellationToken);
            return Ok(updated);
        }

        [HttpGet("{boardId:guid}/analytics")]
        public async Task<ActionResult<BoardAnalyticsDto>> GetBoardAnalytics(
            Guid companyId,
            Guid boardId,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var analytics = await _boardInfoService.GetBoardAnalyticsAsync(boardId, userId, cancellationToken);
            return Ok(analytics);
        }

        [HttpGet("search-tasks")]
        public async Task<ActionResult<List<GlobalSearchResultDto>>> GlobalTaskSearch(
            [FromRoute] Guid companyId,
            [FromQuery] string query,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<GlobalSearchResultDto>());

            var results = await _boardService.GlobalSearchTasksAsync(query, userId, cancellationToken);
            return Ok(results);
        }
    }
}
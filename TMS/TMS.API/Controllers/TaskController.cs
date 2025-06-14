using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.Abstractions.Models.DTOs;
using TMS.Application.Models.DTOs.Task;

namespace TMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService<TaskDto, TaskCreateDto> _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            ITaskService<TaskDto, TaskCreateDto> taskService,
            ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetByIdAsync(id, cancellationToken);
            if (task == null)
            {
                _logger.LogWarning("Task with id {Id} not found", id);
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create([FromBody] TaskCreateDto createDto, CancellationToken cancellationToken)
        {
            if (createDto == null)
            {
                _logger.LogWarning("Create called with null DTO");
                return BadRequest("Task data is required.");
            }

            var task = await _taskService.CreateAsync(createDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskDto updateDto, CancellationToken cancellationToken)
        {
            if (updateDto == null)
            {
                _logger.LogWarning("Update called with null DTO");
                return BadRequest("Task data is required.");
            }
            if (id != updateDto.Id)
            {
                _logger.LogWarning("Update id mismatch: route id {RouteId}, body id {BodyId}", id, updateDto.Id);
                return BadRequest("ID mismatch");
            }

            await _taskService.UpdateAsync(updateDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _taskService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }


        [HttpPost("{taskId:guid}/comments")]
        public async Task<ActionResult<CommentDto>> AddComment(Guid taskId, [FromBody] CommentCreateDto model, CancellationToken cancellationToken)
        {
            if (model == null)
                return BadRequest("Comment data is required.");

            var comment = await _taskService.AddCommentAsync(taskId, model, cancellationToken);
            return Ok(comment);
        }

        [HttpGet("{taskId:guid}/comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(Guid taskId, CancellationToken cancellationToken)
        {
            var comments = await _taskService.GetCommentsAsync(taskId, cancellationToken);
            return Ok(comments);
        }

        [HttpPut("{taskId:guid}/comments/{commentId:guid}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(Guid taskId, Guid commentId, [FromBody] CommentDto model, CancellationToken cancellationToken)
        {
            if (model == null || commentId != model.Id)
                return BadRequest("Invalid data.");

            var updated = await _taskService.UpdateCommentAsync(taskId, commentId, model, cancellationToken);
            return Ok(updated);
        }

        [HttpDelete("{taskId:guid}/comments/{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid taskId, Guid commentId, CancellationToken cancellationToken)
        {
            await _taskService.DeleteCommentAsync(taskId, commentId, cancellationToken);
            return NoContent();
        }
    }
}
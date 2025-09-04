using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Comment;
using TMS.Application.Dto.Task;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for managing Task entities.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskController"/> class.
        /// </summary>
        /// <param name="taskService">The task service.</param>
        /// <param name="logger">The logger.</param>
        public TaskController(
            ITaskService taskService,
            ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The task DTO.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskDto>> GetById(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetByIdAsync(id, userId, cancellationToken);
            if (task == null)
            {
                _logger.LogWarning("Task with id {Id} not found", id);
                return NotFound();
            }
            return Ok(task);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="request">The request containing the task data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created task DTO.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TaskDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CreatedTaskDto>> Create([FromBody] TaskCreateDto request, Guid userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Create called with null DTO");
                return BadRequest("Task data is required.");
            }

            var createDto = new TaskCreateDto
            {
                Name = request.Name,
                TaskTypeId = request.TaskTypeId,
                BoardId = request.BoardId,
                AssigneeId = request.AssigneeId,
                CreatorId = userId,
                ColumnId = request.ColumnId,
            };

            var task = await _taskService.CreateAsync(createDto, userId, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The ID of the task to update.</param>
        /// <param name="request">The request containing the updated task data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskDto request, Guid userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Update called with null DTO");
                return BadRequest("Task data is required.");
            }
            if (id != request.Id)
            {
                _logger.LogWarning("Update id mismatch: route id {RouteId}, body id {BodyId}", id, request.Id);
                return BadRequest("ID mismatch");
            }

            var updateDto = new TaskDto
            {
                Id = request.Id,
                Name = request.Name,
                TaskTypeId = request.TaskTypeId,
                BoardId = request.BoardId,
                ColumnId = request.ColumnId,
            };


            await _taskService.UpdateAsync(updateDto, userId, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            await _taskService.DeleteAsync(id, userId, cancellationToken);
            return NoContent();
        }


        /// <summary>
        /// Adds a comment to a task.
        /// </summary>
        /// <param name="taskId">The ID of the task to add the comment to.</param>
        /// <param name="request">The comment data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created comment DTO.</returns>
        [HttpPost("{taskId:guid}/comments")]
        [ProducesResponseType(typeof(CommentDto), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CommentDto>> AddComment(Guid taskId, [FromBody] CommentDto request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("AddComment called with null model");
                return BadRequest("Comment data is required.");
            }

            var model = new CommentCreateDto
            {
                Text = request.Text,
                AuthorId = request.AuthorId // Assuming CreatedById exists in CreateCommentRequest
            };

            var comment = await _taskService.AddCommentAsync(taskId, model, cancellationToken);
            return Ok(comment);
        }

        /// <summary>
        /// Gets the comments for a task.
        /// </summary>
        /// <param name="taskId">The ID of the task to get the comments for.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The collection of comment DTOs.</returns>
        [HttpGet("{taskId:guid}/comments")]
        [ProducesResponseType(typeof(IEnumerable<CommentDto>), 200)]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(Guid taskId, Guid userId, CancellationToken cancellationToken)
        {
            var comments = await _taskService.GetCommentsAsync(taskId, userId, cancellationToken);
            return Ok(comments);
        }

        /// <summary>
        /// Updates a comment.
        /// </summary>
        /// <param name="taskId">The ID of the task that owns the comment.</param>
        /// <param name="commentId">The ID of the comment to update.</param>
        /// <param name="request">The updated comment data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated comment DTO.</returns>
        [HttpPut("{taskId:guid}/comments/{commentId:guid}")]
        [ProducesResponseType(typeof(CommentDto), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CommentDto>> UpdateComment(Guid taskId, Guid commentId, [FromBody] CommentDto request, CancellationToken cancellationToken)
        {
            if (request == null || commentId != request.Id)
            {
                _logger.LogWarning("UpdateComment called with invalid model or ID mismatch. TaskId: {TaskId}, CommentId: {CommentId}", taskId, commentId);
                return BadRequest("Invalid data.");
            }

            var model = new CommentDto
            {
                Id = request.Id,
                Text = request.Text,
                AuthorId = request.AuthorId // Assuming CreatedById exists in CreateCommentRequest
            };

            var updated = await _taskService.UpdateCommentAsync(taskId, commentId, model, cancellationToken);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes a comment.
        /// </summary>
        /// <param name="taskId">The ID of the task that owns the comment.</param>
        /// <param name="commentId">The ID of the comment to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpDelete("{taskId:guid}/comments/{commentId:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteComment(Guid taskId, Guid commentId, Guid userId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting comment with id {CommentId} from task {TaskId}", commentId, taskId);
            await _taskService.DeleteCommentAsync(taskId, commentId, userId, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Moves a task to another column.
        /// </summary>
        [HttpPost("{taskId:guid}/move")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> MoveTaskToColumn(Guid taskId, [FromQuery] Guid columnId, [FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            if (taskId == Guid.Empty || columnId == Guid.Empty)
            {
                _logger.LogWarning("MoveTask called with invalid parameters");
                return BadRequest("TaskId and ColumnId are required.");
            }

            await _taskService.MoveTaskToColumn(taskId, columnId, userId, cancellationToken);
            return NoContent();
        }

        [HttpGet("my")]
        [ProducesResponseType(typeof(List<TaskDto>), 200)]
        public async Task<ActionResult<List<TaskDto>>> GetMyTasks(Guid userId, CancellationToken cancellationToken)
        {
            var tasks = await _taskService.GetTasksByAssigneeIdAsync(userId, userId, cancellationToken);
            return Ok(tasks);
        }

        [HttpGet("{taskId:guid}/files")]
        [ProducesResponseType(typeof(List<TaskFileDto>), 200)]
        public async Task<ActionResult<List<TaskFileDto>>> GetFiles(Guid taskId, Guid userId, CancellationToken cancellationToken)
        {
            var files = await _taskService.GetFilesAsync(taskId, userId, cancellationToken);
            return Ok(files);
        }

        [HttpDelete("{taskId:guid}/files/{fileId:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteFile(Guid taskId, Guid fileId, Guid userId, CancellationToken cancellationToken)
        {
            await _taskService.DeleteFileAsync(taskId, fileId, userId, cancellationToken);
            return NoContent();
        }

        [HttpGet("{taskId:guid}/files/{fileId:guid}/download")]
        public async Task<IActionResult> DownloadFile(Guid taskId, Guid fileId, Guid userId, CancellationToken cancellationToken)
        {
            var fileDto = await _taskService.DownloadFileAsync(taskId, fileId, userId, cancellationToken);
            if (fileDto == null || fileDto.FileData == null)
                return NotFound();

            return File(fileDto.FileData, fileDto.ContentType, fileDto.FileName);
        }
    }
}
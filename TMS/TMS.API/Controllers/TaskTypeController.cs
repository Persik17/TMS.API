using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.API.ViewModels.TaskType;
using TMS.Application.DTOs.TaskType;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for managing task types.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TaskTypeController : ControllerBase
    {
        private readonly ITaskTypeService<TaskTypeDto, TaskTypeCreateDto> _service;
        private readonly ILogger<TaskTypeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeController"/> class.
        /// </summary>
        /// <param name="service">The task type service.</param>
        /// <param name="logger">The logger.</param>
        public TaskTypeController(
            ITaskTypeService<TaskTypeDto, TaskTypeCreateDto> service,
            ILogger<TaskTypeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a task type by its ID.
        /// </summary>
        /// <param name="id">The ID of the task type to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The task type view model.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TaskTypeViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskTypeViewModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.GetByIdAsync(id, cancellationToken);
            if (dto == null)
            {
                _logger.LogWarning("TaskType with id {Id} not found", id);
                return NotFound();
            }

            return Ok(new TaskTypeViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                CreationDate = dto.CreationDate,
                UpdateDate = dto.UpdateDate,
                DeleteDate = dto.DeleteDate
            });
        }

        /// <summary>
        /// Creates a new task type.
        /// </summary>
        /// <param name="request">The request containing the task type data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created task type view model.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TaskTypeViewModel), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TaskTypeViewModel>> Create([FromBody] TaskTypeCreateViewModel request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Create called with null model");
                return BadRequest("Data is required.");
            }


            var dto = new TaskTypeCreateDto
            {
                Name = request.Name,
                Description = request.Description
            };

            var created = await _service.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new TaskTypeViewModel
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                CreationDate = created.CreationDate,
                UpdateDate = created.UpdateDate,
                DeleteDate = created.DeleteDate
            });
        }

        /// <summary>
        /// Updates an existing task type.
        /// </summary>
        /// <param name="id">The ID of the task type to update.</param>
        /// <param name="request">The request containing the updated task type data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskTypeViewModel request, CancellationToken cancellationToken)
        {
            if (request == null || id != request.Id)
            {
                _logger.LogWarning("Update called with invalid model or ID mismatch. Id: {Id}", id);
                return BadRequest("Invalid data.");
            }


            var dto = new TaskTypeDto
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                CreationDate = request.CreationDate,
                UpdateDate = request.UpdateDate,
                DeleteDate = request.DeleteDate
            };

            await _service.UpdateAsync(dto, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Deletes a task type.
        /// </summary>
        /// <param name="id">The ID of the task type to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting TaskType with id {Id}", id);
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
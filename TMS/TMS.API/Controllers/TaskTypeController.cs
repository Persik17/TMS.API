using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.API.ViewModels.TaskType;
using TMS.Application.Models.DTOs.TaskType;

namespace TMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskTypeController : ControllerBase
    {
        private readonly ITaskTypeService<TaskTypeDto, TaskTypeCreateDto> _service;
        private readonly ILogger<TaskTypeController> _logger;

        public TaskTypeController(
            ITaskTypeService<TaskTypeDto, TaskTypeCreateDto> service,
            ILogger<TaskTypeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskTypeViewModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.GetByIdAsync(id, cancellationToken);
            if (dto == null)
                return NotFound();

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

        [HttpPost]
        public async Task<ActionResult<TaskTypeViewModel>> Create([FromBody] TaskTypeCreateViewModel model, CancellationToken cancellationToken)
        {
            if (model == null)
                return BadRequest("Data is required.");

            var dto = new TaskTypeCreateDto
            {
                Name = model.Name,
                Description = model.Description
            };

            var created = await _service.CreateAsync(dto, cancellationToken);

            return Ok(new TaskTypeViewModel
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                CreationDate = created.CreationDate,
                UpdateDate = created.UpdateDate,
                DeleteDate = created.DeleteDate
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TaskTypeViewModel>> Update(Guid id, [FromBody] TaskTypeViewModel model, CancellationToken cancellationToken)
        {
            if (model == null || id != model.Id)
                return BadRequest("Invalid data.");

            var dto = new TaskTypeDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CreationDate = model.CreationDate,
                UpdateDate = model.UpdateDate,
                DeleteDate = model.DeleteDate
            };

            var updated = await _service.UpdateAsync(dto, cancellationToken);

            return Ok(new TaskTypeViewModel
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description,
                CreationDate = updated.CreationDate,
                UpdateDate = updated.UpdateDate,
                DeleteDate = updated.DeleteDate
            });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
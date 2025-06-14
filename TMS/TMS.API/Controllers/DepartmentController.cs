using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.API.ViewModels.Department;
using TMS.Application.Models.DTOs.Department;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for managing Department entities via HTTP API.
    /// Provides endpoints for CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService<DepartmentDto, DepartmentCreateDto> _departmentService;
        private readonly ILogger<DepartmentController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentController"/> class.
        /// </summary>
        /// <param name="departmentService">Service for department operations.</param>
        /// <param name="logger">Logger instance for diagnostics.</param>
        public DepartmentController(
            IDepartmentService<DepartmentDto, DepartmentCreateDto> departmentService,
            ILogger<DepartmentController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a department by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the department.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// <response code="200">Returns the department view model.</response>
        /// <response code="404">If the department is not found.</response>
        /// </returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DepartmentViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DepartmentViewModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request to get department with id {Id}", id);

            var dto = await _departmentService.GetByIdAsync(id, cancellationToken);
            if (dto == null)
            {
                _logger.LogWarning("Department with id {Id} not found", id);
                return NotFound();
            }

            var viewModel = new DepartmentViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                CompanyName = "", // Можно получить через отдельный сервис, если потребуется
                HeadName = "",    // Можно получить через отдельный сервис, если потребуется
                Description = dto.Description,
                IsActive = dto.IsActive,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                CreationDate = dto.CreationDate,
                UpdateDate = dto.UpdateDate,
                DeleteDate = dto.DeleteDate
            };

            return Ok(viewModel);
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="createDto">The DTO containing department data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// <response code="201">Returns the created department view model.</response>
        /// <response code="400">If the input data is invalid.</response>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(DepartmentViewModel), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DepartmentViewModel>> Create([FromBody] DepartmentCreateDto createDto, CancellationToken cancellationToken)
        {
            if (createDto == null)
            {
                _logger.LogWarning("Create called with null DTO");
                return BadRequest("Department data is required.");
            }

            _logger.LogInformation("Creating department: {Name}", createDto.Name);

            var dto = await _departmentService.CreateAsync(createDto, cancellationToken);
            var viewModel = new DepartmentViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                CompanyName = "", // Можно получить через отдельный сервис, если потребуется
                HeadName = "",    // Можно получить через отдельный сервис, если потребуется
                Description = dto.Description,
                IsActive = dto.IsActive,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                CreationDate = dto.CreationDate,
                UpdateDate = dto.UpdateDate,
                DeleteDate = dto.DeleteDate
            };
            _logger.LogInformation("Department created with id {Id}", dto.Id);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, viewModel);
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="id">The unique identifier of the department.</param>
        /// <param name="updateDto">The DTO containing updated department data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the id in the route and body do not match.</response>
        /// </returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DepartmentDto updateDto, CancellationToken cancellationToken)
        {
            if (updateDto == null)
            {
                _logger.LogWarning("Update called with null DTO");
                return BadRequest("Department data is required.");
            }

            if (id != updateDto.Id)
            {
                _logger.LogWarning("Update id mismatch: route id {RouteId}, body id {BodyId}", id, updateDto.Id);
                return BadRequest("ID mismatch");
            }

            _logger.LogInformation("Updating department with id {Id}", id);

            await _departmentService.UpdateAsync(updateDto, cancellationToken);

            _logger.LogInformation("Department with id {Id} updated", id);

            return NoContent();
        }

        /// <summary>
        /// Deletes a department by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the department to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// <response code="204">If the deletion is successful.</response>
        /// </returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting department with id {Id}", id);

            await _departmentService.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Department with id {Id} deleted", id);

            return NoContent();
        }
    }
}
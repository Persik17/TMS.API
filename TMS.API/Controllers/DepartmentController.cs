using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.API.ViewModels.Department;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Department;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for managing Department entities via HTTP API.
    /// Provides endpoints for CRUD operations.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentController"/> class.
        /// </summary>
        /// <param name="departmentService">The department service.</param>
        /// <param name="logger">The logger.</param>
        public DepartmentController(
            IDepartmentService departmentService,
            ILogger<DepartmentController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a department by its ID.
        /// </summary>
        /// <param name="id">The ID of the department to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The department view model.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DepartmentViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DepartmentViewModel>> GetById(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request to get department with id {Id}", id);

            var dto = await _departmentService.GetByIdAsync(id, userId, cancellationToken);
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
        /// <param name="request">The request containing the department data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created department view model.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(DepartmentViewModel), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DepartmentViewModel>> Create([FromBody] DepartmentViewModel request, Guid userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Create called with null DTO");
                return BadRequest("Department data is required.");
            }

            _logger.LogInformation("Creating department: {Name}", request.Name);

            var createDto = new DepartmentCreateDto
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone
            };

            var dto = await _departmentService.CreateAsync(createDto, userId, cancellationToken);
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
        /// <param name="id">The ID of the department to update.</param>
        /// <param name="request">The request containing the updated department data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DepartmentViewModel request, Guid userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Update called with null DTO");
                return BadRequest("Department data is required.");
            }

            if (id != request.Id)
            {
                _logger.LogWarning("Update id mismatch: route id {RouteId}, body id {BodyId}", id, request.Id);
                return BadRequest("ID mismatch");
            }

            _logger.LogInformation("Updating department with id {Id}", id);

            var updateDto = new DepartmentDto
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone,
                CreationDate = request.CreationDate,
                UpdateDate = request.UpdateDate,
                DeleteDate = request.DeleteDate
            };

            await _departmentService.UpdateAsync(updateDto, userId, cancellationToken);

            _logger.LogInformation("Department with id {Id} updated", id);

            return NoContent();
        }

        /// <summary>
        /// Deletes a department.
        /// </summary>
        /// <param name="id">The ID of the department to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting department with id {Id}", id);

            await _departmentService.DeleteAsync(id, userId, cancellationToken);

            _logger.LogInformation("Department with id {Id} deleted", id);

            return NoContent();
        }
    }
}
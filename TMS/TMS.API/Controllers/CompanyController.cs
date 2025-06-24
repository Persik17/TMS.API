using Microsoft.AspNetCore.Mvc;
using TMS.Abstractions.Interfaces.Services;
using TMS.API.ViewModels.Company;
using TMS.Application.DTOs.Company;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for managing company entities.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService<CompanyDto, CompanyCreateDto> _companyService;
        private readonly ILogger<CompanyController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyController"/> class.
        /// </summary>
        /// <param name="companyService">The company service.</param>
        /// <param name="logger">The logger.</param>
        public CompanyController(
            ICompanyService<CompanyDto, CompanyCreateDto> companyService,
            ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a company by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the company.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The company view model.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyViewModel>> GetCompany(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Request to get company with id {Id}", id);

            var companyDto = await _companyService.GetByIdAsync(id, cancellationToken);

            if (companyDto == null)
            {
                _logger.LogWarning("Company with id {Id} not found", id);
                return NotFound($"Company with id {id} not found.");
            }

            CompanyViewModel companyViewModel = new()
            {
                Name = companyDto.Name
            };

            return Ok(companyViewModel);
        }

        /// <summary>
        /// Creates a new company.
        /// </summary>
        /// <param name="request">The request containing the company data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created company view model.</returns>
        [HttpPost]
        public async Task<ActionResult<CompanyViewModel>> CreateCompany([FromBody] CompanyViewModel request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                _logger.LogWarning("CreateCompany called with null DTO");
                return BadRequest("Company data is required.");
            }

            _logger.LogInformation("Creating company: {Name}", request.Name);

            var companyCreateDto = new CompanyCreateDto
            {
                Name = request.Name
            };

            var createdCompanyDto = await _companyService.CreateAsync(companyCreateDto, cancellationToken);

            CompanyViewModel companyViewModel = new()
            {
                Name = createdCompanyDto.Name
            };

            _logger.LogInformation("Company created with id {Id}", createdCompanyDto.Id);

            return CreatedAtAction(nameof(GetCompany), new { id = createdCompanyDto.Id }, companyViewModel);
        }

        /// <summary>
        /// Updates an existing company.
        /// </summary>
        /// <param name="id">The unique identifier of the company.</param>
        /// <param name="request">The request containing updated company data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated company view model.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyViewModel>> UpdateCompany(Guid id, [FromBody] CompanyViewModel request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                _logger.LogWarning("UpdateCompany called with null DTO");
                return BadRequest("Company data is required.");
            }

            if (id != request.Id)
            {
                _logger.LogWarning("UpdateCompany id mismatch: route id {RouteId}, body id {BodyId}", id, request.Id);
                return BadRequest("ID in the route and body must match.");
            }

            _logger.LogInformation("Updating company with id {Id}", id);

            var companyDto = new CompanyDto
            {
                Id = request.Id,
                Name = request.Name
            };

            var updatedCompanyDto = await _companyService.UpdateAsync(companyDto, cancellationToken);

            CompanyViewModel companyViewModel = new()
            {
                Name = updatedCompanyDto.Name
            };

            _logger.LogInformation("Company with id {Id} updated", id);

            return Ok(companyViewModel);
        }

        /// <summary>
        /// Deletes a company by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the company to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting company with id {Id}", id);

            await _companyService.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Company with id {Id} deleted", id);

            return NoContent();
        }
    }
}
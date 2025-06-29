using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.API.ViewModels.Company;
using TMS.Application.Abstractions.Services;
using TMS.Application.Dto.Company;

namespace TMS.API.Controllers
{
    /// <summary>
    /// Controller for managing company entities.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyController"/> class.
        /// </summary>
        public CompanyController(
            ICompanyService companyService,
            ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a company by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the company.</param>
        /// <param name="userId">Id of the user performing the action.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The company view model.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyViewModel>> GetCompany(
            Guid id,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Request to get company with id {Id} by user {UserId}", id, userId);

            var companyDto = await _companyService.GetByIdAsync(id, userId, cancellationToken);
            if (companyDto == null)
            {
                _logger.LogWarning("Company with id {Id} not found", id);
                return NotFound($"Company with id {id} not found.");
            }

            var viewModel = new CompanyViewModel
            {
                Id = companyDto.Id,
                Name = companyDto.Name,
                INN = companyDto.INN,
                OGRN = companyDto.OGRN,
                Address = companyDto.Address,
                Logo = companyDto.Logo,
                Website = companyDto.Website,
                Industry = companyDto.Industry,
                Description = companyDto.Description,
                OwnerId = companyDto.OwnerId,
                ContactEmail = companyDto.ContactEmail,
                ContactPhone = companyDto.ContactPhone,
                CreationDate = companyDto.CreationDate,
                UpdateDate = companyDto.UpdateDate,
                DeleteDate = companyDto.DeleteDate
            };
            return Ok(viewModel);
        }

        /// <summary>
        /// Creates a new company.
        /// </summary>
        /// <param name="model">The request containing the company data.</param>
        /// <param name="userId">Id of the user performing the action.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created company view model.</returns>
        [HttpPost]
        public async Task<ActionResult<CompanyViewModel>> CreateCompany(
            [FromBody] CompanyCreateViewModel model,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (model == null)
            {
                _logger.LogWarning("CreateCompany called with null DTO");
                return BadRequest("Company data is required.");
            }

            _logger.LogInformation("User {UserId} creating company: {Name}", userId, model.Name);

            var companyCreateDto = new CompanyCreateDto
            {
                Name = model.Name,
                INN = model.INN,
                OGRN = model.OGRN,
                Address = model.Address,
                Logo = model.Logo,
                Website = model.Website,
                Industry = model.Industry,
                Description = model.Description,
                OwnerId = userId,
                ContactEmail = model.ContactEmail,
                ContactPhone = model.ContactPhone
            };

            var createdCompanyDto = await _companyService.CreateAsync(companyCreateDto, userId, cancellationToken);

            var viewModel = new CompanyViewModel
            {
                Id = createdCompanyDto.Id,
                Name = createdCompanyDto.Name,
                INN = createdCompanyDto.INN,
                OGRN = createdCompanyDto.OGRN,
                Address = createdCompanyDto.Address,
                Logo = createdCompanyDto.Logo,
                Website = createdCompanyDto.Website,
                Industry = createdCompanyDto.Industry,
                Description = createdCompanyDto.Description,
                OwnerId = createdCompanyDto.OwnerId,
                ContactEmail = createdCompanyDto.ContactEmail,
                ContactPhone = createdCompanyDto.ContactPhone,
                CreationDate = createdCompanyDto.CreationDate,
                UpdateDate = createdCompanyDto.UpdateDate,
                DeleteDate = createdCompanyDto.DeleteDate
            };

            _logger.LogInformation("Company created with id {Id} by user {UserId}", createdCompanyDto.Id, userId);

            return CreatedAtAction(nameof(GetCompany), new { id = createdCompanyDto.Id, userId }, viewModel);
        }

        /// <summary>
        /// Updates an existing company.
        /// </summary>
        /// <param name="id">The unique identifier of the company.</param>
        /// <param name="model">The request containing updated company data.</param>
        /// <param name="userId">Id of the user performing the action.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated company view model.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyViewModel>> UpdateCompany(
            Guid id,
            [FromBody] CompanyUpdateViewModel model,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (model == null)
            {
                _logger.LogWarning("UpdateCompany called with null DTO");
                return BadRequest("Company data is required.");
            }
            if (id != model.Id)
            {
                _logger.LogWarning("UpdateCompany id mismatch: route id {RouteId}, body id {BodyId}", id, model.Id);
                return BadRequest("ID in the route and body must match.");
            }

            _logger.LogInformation("User {UserId} updating company with id {Id}", userId, id);

            var companyDto = new CompanyDto
            {
                Id = model.Id,
                Name = model.Name,
                INN = model.INN,
                OGRN = model.OGRN,
                Address = model.Address,
                Logo = model.Logo,
                Website = model.Website,
                Industry = model.Industry,
                Description = model.Description,
                OwnerId = model.OwnerId,
                ContactEmail = model.ContactEmail,
                ContactPhone = model.ContactPhone,
                CreationDate = model.CreationDate,
                UpdateDate = model.UpdateDate,
                DeleteDate = model.DeleteDate
            };

            var updatedCompanyDto = await _companyService.UpdateAsync(companyDto, userId, cancellationToken);

            var viewModel = new CompanyViewModel
            {
                Id = updatedCompanyDto.Id,
                Name = updatedCompanyDto.Name,
                INN = updatedCompanyDto.INN,
                OGRN = updatedCompanyDto.OGRN,
                Address = updatedCompanyDto.Address,
                Logo = updatedCompanyDto.Logo,
                Website = updatedCompanyDto.Website,
                Industry = updatedCompanyDto.Industry,
                Description = updatedCompanyDto.Description,
                OwnerId = updatedCompanyDto.OwnerId,
                ContactEmail = updatedCompanyDto.ContactEmail,
                ContactPhone = updatedCompanyDto.ContactPhone,
                CreationDate = updatedCompanyDto.CreationDate,
                UpdateDate = updatedCompanyDto.UpdateDate,
                DeleteDate = updatedCompanyDto.DeleteDate
            };

            _logger.LogInformation("Company with id {Id} updated by user {UserId}", id, userId);

            return Ok(viewModel);
        }

        /// <summary>
        /// Deletes a company by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the company to delete.</param>
        /// <param name="userId">Id of the user performing the action.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(
            Guid id,
            [FromQuery] Guid userId,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("User {UserId} deleting company with id {Id}", userId, id);

            await _companyService.DeleteAsync(id, userId, cancellationToken);

            _logger.LogInformation("Company with id {Id} deleted by user {UserId}", id, userId);

            return NoContent();
        }
    }
}
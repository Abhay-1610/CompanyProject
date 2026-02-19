using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyProject.Application.Companies.CreateCompany;
using CompanyProject.Application.Companies.UpdateCompany;
using CompanyProject.Application.Companies.DeleteCompany;
using CompanyProject.Application.Companies.GetCompanies;

namespace CompanyProject.Api.Controllers.Companies
{
    // ======================================================
    // Companies API Controller
    // Accessible only to authenticated users
    // ======================================================
    [ApiController]
    [Route("api/companies")]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        // ==================================================
        // Constructor (MediatR injection)
        // ==================================================
        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ==================================================
        // GET: api/companies
        // SuperAdmin only
        // ==================================================
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetCompaniesQuery());
            return Ok(result);
        }

        // ==================================================
        // POST: api/companies
        // Create new company (SuperAdmin only)
        // ==================================================
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(
            [FromBody] CreateCompanyCommand command)
        {
            var company = await _mediator.Send(command);
            return Ok(company);
        }

        // ==================================================
        // PUT: api/companies/{id}
        // Update existing company (SuperAdmin only)
        // ==================================================
        [HttpPut("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCompanyCommand command)
        {
            command.CompanyId = id; // route id → command
            var company = await _mediator.Send(command);
            return Ok(company);
        }

        // ==================================================
        // DELETE: api/companies/{id}
        // Delete company (SuperAdmin only)
        // ==================================================
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(
                new DeleteCompanyCommand { CompanyId = id }
            );

            return NoContent();
        }
    }
}

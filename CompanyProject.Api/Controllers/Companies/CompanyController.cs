using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyProject.Application.Companies.CreateCompany;
using CompanyProject.Application.Companies.UpdateCompany;
using CompanyProject.Application.Companies.DeleteCompany;
using CompanyProject.Application.Companies.GetCompanies;

namespace CompanyProject.Api.Controllers.Companies
{
    [ApiController]
    [Route("api/companies")]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetCompaniesQuery());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
        {
            var companyId = await _mediator.Send(command);
            return Ok(companyId);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateCompanyCommand command)
        {
            command.CompanyId = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCompanyCommand { CompanyId = id });
            return NoContent();
        }
    }
}

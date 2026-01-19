using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyProject.Application.History.GetCompanyAudit;

namespace CompanyProject.Api.Controllers.ChangeHistory
{
    [ApiController]
    [Route("api/change-history")]
    [Authorize]
    public class ChangeHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChangeHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "CompanyAdmin")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetCompanyAuditQuery());

            return Ok(result);
        }
    }
}

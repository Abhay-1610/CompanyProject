using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyProject.Application.History.GetCompanyAudit;
using CompanyProject.Application.History.GetAllAudit;

namespace CompanyProject.Api.Controllers.ChangeHistory
{
    // ======================================================
    // Change History (Audit Logs) API Controller
    // ======================================================
    [ApiController]
    [Route("api/change-history")]
    [Authorize]
    public class ChangeHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        // ==================================================
        // Constructor (MediatR)
        // ==================================================
        public ChangeHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ==================================================
        // GET: api/change-history
        // Company-level audit (current user's company)
        // ==================================================
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(
                new GetCompanyAuditQuery()
            );

            return Ok(result);
        }

        // ==================================================
        // GET: api/change-history/all
        // All audit logs (SuperAdmin only)
        // ==================================================
        [HttpGet("all")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllAuditsQuery()
            );

            return Ok(result);
        }
    }
}

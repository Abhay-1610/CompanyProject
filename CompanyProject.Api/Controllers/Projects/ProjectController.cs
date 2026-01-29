using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyProject.Application.Projects.CreateProject;
using CompanyProject.Application.Projects.UpdateProject;
using CompanyProject.Application.Projects.DeleteProject;
using CompanyProject.Application.Projects.GetProjects;

namespace CompanyProject.Api.Controllers.Projects
{
    // ======================================================
    // Projects API Controller
    // Requires authenticated user
    // ======================================================
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        // ==================================================
        // Constructor (MediatR)
        // ==================================================
        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ==================================================
        // GET: api/projects
        // Optional filter by companyId
        // ==================================================
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int? companyId)
        {
            var result = await _mediator.Send(
                new GetProjectsQuery { CompanyId = companyId }
            );

            return Ok(result);
        }

        // ==================================================
        // POST: api/projects
        // Create new project
        // ==================================================
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateProjectCommand command)
        {
            var project = await _mediator.Send(command);
            return Ok(project);
        }

        // ==================================================
        // PUT: api/projects/{id}
        // Update existing project
        // ==================================================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateProjectCommand command)
        {
            command.ProjectId = id; // route id → command
            var project = await _mediator.Send(command);
            return Ok(project);
        }

        // ==================================================
        // DELETE: api/projects/{id}
        // Delete project
        // ==================================================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(
                new DeleteProjectCommand { ProjectId = id }
            );

            return NoContent();
        }
    }
}

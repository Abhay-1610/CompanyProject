using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyProject.Application.Projects.CreateProject;
using CompanyProject.Application.Projects.UpdateProject;
using CompanyProject.Application.Projects.DeleteProject;
using CompanyProject.Application.Projects.GetProjects;

namespace CompanyProject.Api.Controllers.Projects
{
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetProjectsQuery());
            return Ok(result);
        }

   
        [HttpPost]
        [Authorize(Roles = "CompanyAdmin,CompanyUser")]
        public async Task<IActionResult> Create([FromBody] CreateProjectCommand command)
        {
            var projectId = await _mediator.Send(command);
            return Ok(projectId);
        }

   
        [HttpPut("{id:int}")]
        [Authorize(Roles = "CompanyAdmin,CompanyUser")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateProjectCommand command)
        {
            command.ProjectId = id;
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "CompanyAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProjectCommand { ProjectId = id });
            return NoContent();
        }
    }
}

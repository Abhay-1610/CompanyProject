using CompanyProject.Application.Common.Dtos;
using MediatR;

namespace CompanyProject.Application.Projects.CreateProject
{
    public class CreateProjectCommand : IRequest<ProjectDto>
    {
        public string ProjectName { get; set; } = string.Empty;
        public string CopanyName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CompanyId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

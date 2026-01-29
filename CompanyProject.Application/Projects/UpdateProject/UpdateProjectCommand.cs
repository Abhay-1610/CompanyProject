using CompanyProject.Application.Common.Dtos;
using MediatR;

namespace CompanyProject.Application.Projects.UpdateProject
{
    public class UpdateProjectCommand : IRequest<ProjectDto>
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CompanyId { get; set; }

        public string Status { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; }
    }
}

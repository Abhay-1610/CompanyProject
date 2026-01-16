using MediatR;

namespace CompanyProject.Application.Projects.CreateProject
{
    public class CreateProjectCommand : IRequest<int>
    {
        public string ProjectName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

using MediatR;

namespace CompanyProject.Application.Projects.DeleteProject
{
    public class DeleteProjectCommand : IRequest
    {
        public int ProjectId { get; set; }
    }
}

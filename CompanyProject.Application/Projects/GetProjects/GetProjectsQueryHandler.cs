using MediatR;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;

namespace CompanyProject.Application.Projects.GetProjects
{
    public class GetProjectsQueryHandler
        : IRequestHandler<GetProjectsQuery, List<Project>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUser _currentUser;

        public GetProjectsQueryHandler(
            IProjectRepository projectRepository,
            ICurrentUser currentUser)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task<List<Project>> Handle(
            GetProjectsQuery request,
            CancellationToken cancellationToken)
        {
            return await _projectRepository
                .GetByCompanyIdAsync(_currentUser.CompanyId);
        }
    }
}

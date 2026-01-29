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
            int companyId;

            if (_currentUser.Role=="SuperAdmin")
            {
                // SuperAdmin MUST send companyId explicitly
                companyId = request.CompanyId
                    ?? throw new ArgumentException("CompanyId is required for SuperAdmin");
            }
            else
            {
                // CompanyAdmin / CompanyUser → always from token
                companyId = _currentUser.CompanyId
                    ?? throw new UnauthorizedAccessException();
            }

            return await _projectRepository.GetByCompanyIdAsync(companyId);
        }

    }
}

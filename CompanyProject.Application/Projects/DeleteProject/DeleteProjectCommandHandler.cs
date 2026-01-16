using MediatR;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Application.Projects.DeleteProject
{
    public class DeleteProjectCommandHandler
        : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUser _currentUser;

        public DeleteProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUser currentUser)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task Handle(
            DeleteProjectCommand request,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);

            if (project == null)
                throw new Exception("Project not found");

            if (_currentUser.IsBlocked)
                throw new Exception("Operation blocked. Contact your Company Admin.");

            if (project.CompanyId != _currentUser.CompanyId)
                throw new Exception("Access denied");

            await _projectRepository.DeleteAsync(project);
        }
    }
}

using MediatR;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Application.Projects.UpdateProject
{
    public class UpdateProjectCommandHandler
        : IRequestHandler<UpdateProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUser _currentUser;

        public UpdateProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUser currentUser)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task Handle(
            UpdateProjectCommand request,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);

            if (project == null)
                throw new Exception("Project not found");

            if (_currentUser.IsBlocked)
                throw new Exception("Operation blocked. Contact your Company Admin.");

            if (project.CompanyId != _currentUser.CompanyId)
                throw new Exception("Access denied");

            project.ProjectName = request.ProjectName;
            project.Description = request.Description;
            project.Status = request.Status;
            project.StartDate = request.StartDate;
            project.EndDate = request.EndDate;

            await _projectRepository.UpdateAsync(project);
        }
    }
}

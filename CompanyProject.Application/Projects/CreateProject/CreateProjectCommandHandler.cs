using MediatR;
using CompanyProject.Domain.Entities;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Application.Projects.CreateProject
{
    public class CreateProjectCommandHandler
        : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUser _currentUser;

        public CreateProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUser currentUser)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(
            CreateProjectCommand request,
            CancellationToken cancellationToken)
        {
            var project = new Project
            {
                ProjectName = request.ProjectName,
                Description = request.Description,
                CompanyId = _currentUser.CompanyId,
                Status = "InProgress",
                IsActive = true,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedByUserId = _currentUser.UserId
            };

            await _projectRepository.AddAsync(project);

            return project.ProjectId;
        }
    }
}

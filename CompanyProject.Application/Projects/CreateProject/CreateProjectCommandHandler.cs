using CompanyProject.Application.Common.Dtos;
using CompanyProject.Application.History.Create;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using FluentValidation;
using MediatR;

namespace CompanyProject.Application.Projects.CreateProject
{
    public class CreateProjectCommandHandler
        : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMediator _mediator;

        public CreateProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUser currentUser, IMediator mediator, IValidator<CreateProjectCommand> validator)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
            _mediator = mediator;
        }

        public async Task<ProjectDto> Handle(
            CreateProjectCommand request,
            CancellationToken cancellationToken)
        {

            var exists = await _projectRepository.ProjectNameExistsAsync(
        request.ProjectName,
        request.CompanyId
    );

            if (exists)
                throw new InvalidOperationException("Project with this name already exists.");

            var project = new Project
            {
                ProjectName = request.ProjectName,
                Description = request.Description,
                CompanyId = request.CompanyId,
                Status = "InProgress",
                IsActive = true,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedByUserId = _currentUser.UserId
            };

            await _projectRepository.AddAsync(project);

            var newData = System.Text.Json.JsonSerializer.Serialize(project);

            // 2️⃣ Create ChangeHistory
            await _mediator.Send(new CreateChangeHistoryCommand
            {
                ProjectName = request.ProjectName,
                CompanyName = _currentUser.CompanyName,
                companyId = request.CompanyId,
                ProjectId = project.ProjectId,
                ChangeType = "Create",
                OldData = null,
                NewData = newData
            });

            return new ProjectDto
            {
                ProjectId = project.ProjectId,
                CompanyId = request.CompanyId,
                ProjectName = request.ProjectName,
                Description = request.Description,
                Status = "InProgress",
                IsActive = true,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedByUserId = _currentUser.UserId
            };
        }
    }
}

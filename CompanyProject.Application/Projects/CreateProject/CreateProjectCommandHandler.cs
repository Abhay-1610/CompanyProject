using CompanyProject.Application.History.Create;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using FluentValidation;
using MediatR;

namespace CompanyProject.Application.Projects.CreateProject
{
    public class CreateProjectCommandHandler
        : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMediator _mediator;
        private readonly IValidator<CreateProjectCommand> _validator;

        public CreateProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUser currentUser, IMediator mediator, IValidator<CreateProjectCommand> validator)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<int> Handle(
            CreateProjectCommand request,
            CancellationToken cancellationToken)
        {

            await _validator.ValidateAsync(request, cancellationToken);
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

            var newData = System.Text.Json.JsonSerializer.Serialize(project);

            // 2️⃣ Create ChangeHistory
            await _mediator.Send(new CreateChangeHistoryCommand
            {
                ProjectId = project.ProjectId,
                ChangeType = "Create",
                OldData = null,
                NewData = newData
            });

            return project.ProjectId;
        }
    }
}

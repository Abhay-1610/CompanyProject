using CompanyProject.Application.History.Create;
using CompanyProject.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace CompanyProject.Application.Projects.UpdateProject
{
    public class UpdateProjectCommandHandler
        : IRequestHandler<UpdateProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMediator _mediator;
        private readonly IValidator<UpdateProjectCommand> _validator;

        public UpdateProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUser currentUser,
            IMediator mediator, IValidator<UpdateProjectCommand> validator)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task Handle(
            UpdateProjectCommand request,
            CancellationToken cancellationToken)
        {

            await _validator.ValidateAsync(request, cancellationToken);
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);

            if (project == null)
                throw new Exception("Project not found");

            //if (_currentUser.IsBlocked)
            //    throw new Exception("Operation blocked. Contact your Company Admin.");

            if (project.CompanyId != _currentUser.CompanyId)
                throw new Exception("Access denied");

            // Capture old state (simple & sufficient)
            var oldData = System.Text.Json.JsonSerializer.Serialize(project);


            // Update project
            project.ProjectName = request.ProjectName;
            project.Description = request.Description;
            project.Status = request.Status;
            project.StartDate = request.StartDate;
            project.EndDate = request.EndDate;

            await _projectRepository.UpdateAsync(project);

            // Capture new state
            var newData = System.Text.Json.JsonSerializer.Serialize(project);

            // Create audit trail
            await _mediator.Send(new CreateChangeHistoryCommand
            {
                ProjectId = project.ProjectId,
                ChangeType = "Update",
                OldData = oldData,
                NewData = newData
            });
        }
    }
}

using CompanyProject.Application.History.Create;
using CompanyProject.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace CompanyProject.Application.Projects.DeleteProject
{
    public class DeleteProjectCommandHandler
        : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMediator _mediator;
        private readonly IValidator<DeleteProjectCommand> _validator;

        public DeleteProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUser currentUser,
            IMediator mediator, IValidator<DeleteProjectCommand> validator)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task Handle(
            DeleteProjectCommand request,
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

            var oldData = System.Text.Json.JsonSerializer.Serialize(project);

            await _projectRepository.DeleteAsync(project);

            // 2️⃣ Create ChangeHistory
            await _mediator.Send(new CreateChangeHistoryCommand
            {
                ProjectId = project.ProjectId,
                ChangeType = "Delete",
                OldData = oldData,
                NewData = null
            });

        }
    }
}

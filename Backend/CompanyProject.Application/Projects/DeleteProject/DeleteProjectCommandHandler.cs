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
        private readonly IUserRepository _userRepository;


        public DeleteProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUser currentUser,
            IMediator mediator,
            IValidator<DeleteProjectCommand> validator,
            IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
            _mediator = mediator;
            _validator = validator;
            _userRepository = userRepository;
        }

        public async Task Handle(
            DeleteProjectCommand request,
            CancellationToken cancellationToken)
        {
            var isBlocked = await _userRepository.IsUserBlockedAsync(_currentUser.UserId);

            if (isBlocked)
                throw new UnauthorizedAccessException("You are blocked !!, Contact your admin.");
       

            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
                throw new Exception("Project not found");

            // Authorization (unchanged)
            if (!_currentUser.IsSuperAdmin)
            {
                if (project.CompanyId != _currentUser.CompanyId)
                    throw new Exception("Access denied");
            }

            // Capture old state BEFORE delete
            var oldData = System.Text.Json.JsonSerializer.Serialize(project);

            // Delete project

            // Create ChangeHistory (CompanyId passed explicitly)
            await _mediator.Send(new CreateChangeHistoryCommand
            {
                companyId = project.CompanyId,
                CompanyName = _currentUser.CompanyName,
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ChangeType = "Delete",
                OldData = oldData,
                NewData = null
            }, cancellationToken);

            await _projectRepository.DeleteAsync(project);

        }
    }
}

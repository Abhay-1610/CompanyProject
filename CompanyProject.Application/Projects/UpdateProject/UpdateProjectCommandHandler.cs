using CompanyProject.Application.Common.Dtos;
using CompanyProject.Application.History.Create;
using CompanyProject.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CompanyProject.Application.Projects.UpdateProject
{
    public class UpdateProjectCommandHandler
        : IRequestHandler<UpdateProjectCommand,ProjectDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMediator _mediator;
        private readonly IValidator<UpdateProjectCommand> _validator;
        private readonly IUserRepository _userRepository;


        public UpdateProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUser currentUser,
            IMediator mediator, IValidator<UpdateProjectCommand> validator, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
            _mediator = mediator;
            _validator = validator;
            _userRepository = userRepository;
        }

        public async Task<ProjectDto> Handle(
            UpdateProjectCommand request,
            CancellationToken cancellationToken)
        {
            var isBlocked = await _userRepository.IsUserBlockedAsync(_currentUser.UserId);

            if (isBlocked)
                throw new UnauthorizedAccessException("You are blocked !!, Contact your admin.");

            var project = await _projectRepository.GetByIdAsync(request.ProjectId);

            if (project == null)
                throw new Exception("Project not found");

            var exists = await _projectRepository.ProjectNameExistsAsync(request.ProjectName, project.CompanyId, request.ProjectId);

            if (exists)
                throw new InvalidOperationException("Project with this name already exists.");





            if (!_currentUser.IsSuperAdmin && project.CompanyId != _currentUser.CompanyId)
                throw new UnauthorizedAccessException();


            var oldData = System.Text.Json.JsonSerializer.Serialize(project);


            // Update project
            project.ProjectName = request.ProjectName;
            project.Description = request.Description;
            project.Status = "InProgress";
            project.StartDate = request.StartDate;
            project.EndDate = request.EndDate;

            await _projectRepository.UpdateAsync(project);

            // Capture new state
            var newData = System.Text.Json.JsonSerializer.Serialize(project);

            // Create audit trail
            await _mediator.Send(new CreateChangeHistoryCommand
            {
                ProjectId = project.ProjectId,
                ProjectName = request.ProjectName,
                ChangeType = "Update",
                OldData = oldData,
                NewData = newData,
                companyId = project.CompanyId,
                CompanyName = _currentUser.CompanyName
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
                CreatedByUserId = project.CreatedByUserId
            };
        }
    }
}

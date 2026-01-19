using FluentValidation;

namespace CompanyProject.Application.Projects.DeleteProject;

public sealed class DeleteProjectCommandValidator
    : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0);
    }
}

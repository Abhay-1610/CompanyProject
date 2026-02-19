using FluentValidation;

namespace CompanyProject.Application.Projects.UpdateProject;

public sealed class UpdateProjectCommandValidator
    : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0);

        RuleFor(x => x.ProjectName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate);
    }
}

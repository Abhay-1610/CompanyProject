using FluentValidation;

namespace CompanyProject.Application.Projects.CreateProject;

public sealed class CreateProjectCommandValidator
    : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate);
    }
}

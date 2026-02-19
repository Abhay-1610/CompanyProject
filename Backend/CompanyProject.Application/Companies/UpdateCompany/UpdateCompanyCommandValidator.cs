using FluentValidation;

namespace CompanyProject.Application.Companies.UpdateCompany;

public sealed class UpdateCompanyCommandValidator
    : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0);

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .MaximumLength(100);
    }
}

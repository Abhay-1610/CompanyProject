using CompanyProject.Application.Companies.CreateCompany;
using FluentValidation;

public sealed class CreateCompanyCommandValidator
    : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(10);
    }
}

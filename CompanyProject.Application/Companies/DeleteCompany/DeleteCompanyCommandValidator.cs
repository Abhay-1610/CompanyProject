using FluentValidation;

namespace CompanyProject.Application.Companies.DeleteCompany;

public sealed class DeleteCompanyCommandValidator
    : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0);
    }
}

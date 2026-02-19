using CompanyProject.Application.Companies.UpdateCompany;
using CompanyProject.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace CompanyProject.Application.Companies.DeleteCompany
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<DeleteCompanyCommand> _validator;

        public DeleteCompanyCommandHandler(ICompanyRepository companyRepository, IValidator<DeleteCompanyCommand> validator)
        {
            _companyRepository = companyRepository;
            _validator = validator;
        }

        public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAsync(request, cancellationToken);

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            if (company == null)
                throw new Exception("Company not found");

            await _companyRepository.DeleteAsync(company);
        }
    }
}

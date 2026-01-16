using MediatR;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Application.Companies.DeleteCompany
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        public DeleteCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            if (company == null)
                throw new Exception("Company not found");

            await _companyRepository.DeleteAsync(company);
        }
    }
}

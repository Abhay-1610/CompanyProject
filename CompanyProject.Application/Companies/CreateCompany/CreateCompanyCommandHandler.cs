
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using MediatR;

namespace CompanyProject.Application.Companies.CreateCompany
{
    public class CreateCompanyCommandHandler:IRequestHandler<CreateCompanyCommand , int>
    {
        private readonly ICompanyRepository _companyRepository;
        public CreateCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new Company
            {
                CompanyName = request.CompanyName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _companyRepository.AddAsync(company);

            return company.CompanyId;
        }
    }
}

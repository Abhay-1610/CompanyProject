using MediatR;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;

namespace CompanyProject.Application.Companies.GetCompanies
{
    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, List<Company>>
    {
        private readonly ICompanyRepository _companyRepository;

        public GetCompaniesQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<List<Company>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            return await _companyRepository.GetAllAsync();
        }
    }
}

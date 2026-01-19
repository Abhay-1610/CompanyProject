using CompanyProject.Application.Companies.UpdateCompany;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using FluentValidation;
using MediatR;

namespace CompanyProject.Application.Companies.GetCompanies
{
    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, List<Company>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<UpdateCompanyCommand> _validator;
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

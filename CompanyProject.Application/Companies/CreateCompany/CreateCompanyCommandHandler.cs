
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using FluentValidation;
using MediatR;

namespace CompanyProject.Application.Companies.CreateCompany
{
    public class CreateCompanyCommandHandler:IRequestHandler<CreateCompanyCommand , int>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<CreateCompanyCommand> _validator;
        public CreateCompanyCommandHandler(ICompanyRepository companyRepository, IValidator<CreateCompanyCommand> validator)
        {
            _companyRepository = companyRepository;
            _validator = validator;
        }

        public async Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAsync(request, cancellationToken);
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

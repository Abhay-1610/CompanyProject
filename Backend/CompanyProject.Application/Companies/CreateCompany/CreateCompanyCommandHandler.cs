
using CompanyProject.Application.Common.Dtos;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using FluentValidation;
using MediatR;

namespace CompanyProject.Application.Companies.CreateCompany
{
    public class CreateCompanyCommandHandler:IRequestHandler<CreateCompanyCommand , CompanyDto>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<CreateCompanyCommand> _validator;
        public CreateCompanyCommandHandler(ICompanyRepository companyRepository, IValidator<CreateCompanyCommand> validator)
        {
            _companyRepository = companyRepository;
            _validator = validator;
        }

        public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
             var exists = await _companyRepository.CompanyNameExistsAsync(request.CompanyName);
         if (exists)
        throw new InvalidOperationException("Company with this name already exists.");

            var company = new Company
            {
                CompanyName = request.CompanyName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _companyRepository.AddAsync(company);

            return new CompanyDto
            {
                CompanyId = company.CompanyId,
                CompanyName = request.CompanyName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}

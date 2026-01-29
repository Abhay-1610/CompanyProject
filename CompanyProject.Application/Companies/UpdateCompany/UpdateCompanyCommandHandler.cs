using CompanyProject.Application.Common.Dtos;
using CompanyProject.Application.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProject.Application.Companies.UpdateCompany
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand,CompanyDto>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<UpdateCompanyCommand> _validator;
        public UpdateCompanyCommandHandler(ICompanyRepository companyRepository, IValidator<UpdateCompanyCommand> validator)
        {
            _companyRepository = companyRepository;
            _validator = validator;
        }
        public async Task<CompanyDto> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {

            var exists = await _companyRepository.CompanyNameExistsAsync(request.CompanyName);
            if (exists)
                throw new InvalidOperationException("Company with this name already exists.");

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            if (company == null)
                throw new Exception("Company not found");

            company.CompanyName = request.CompanyName;

            await _companyRepository.UpdateAsync(company);

            return new CompanyDto
            {
                CompanyName = company.CompanyName,
                CompanyId = company.CompanyId,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt
            };
        }
    }
}

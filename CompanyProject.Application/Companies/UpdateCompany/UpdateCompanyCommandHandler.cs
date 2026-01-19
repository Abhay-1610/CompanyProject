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
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<UpdateCompanyCommand> _validator;
        public UpdateCompanyCommandHandler(ICompanyRepository companyRepository, IValidator<UpdateCompanyCommand> validator)
        {
            _companyRepository = companyRepository;
            _validator = validator;
        }
        public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {

            await _validator.ValidateAsync(request, cancellationToken);

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            if (company == null)
                throw new Exception("Company not found");

            company.CompanyName = request.CompanyName;

            await _companyRepository.UpdateAsync(company);
        }
    }
}



using CompanyProject.Application.Common.Dtos;
using MediatR;

namespace CompanyProject.Application.Companies.UpdateCompany
{
    public class UpdateCompanyCommand:IRequest<CompanyDto>
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
    }
}

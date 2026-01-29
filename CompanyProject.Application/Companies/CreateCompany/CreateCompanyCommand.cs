using CompanyProject.Application.Common.Dtos;
using MediatR;

namespace CompanyProject.Application.Companies.CreateCompany
{
    public class CreateCompanyCommand : IRequest<CompanyDto>
    {
        public string CompanyName { get; set; } = string.Empty;
        public int CompanyId { get; set; }
    }
}

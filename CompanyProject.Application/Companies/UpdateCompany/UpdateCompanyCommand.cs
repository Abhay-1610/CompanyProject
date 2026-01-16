

using MediatR;

namespace CompanyProject.Application.Companies.UpdateCompany
{
    public class UpdateCompanyCommand:IRequest
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
    }
}

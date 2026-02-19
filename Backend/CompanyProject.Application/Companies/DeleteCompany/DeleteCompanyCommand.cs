using MediatR;

namespace CompanyProject.Application.Companies.DeleteCompany
{
    public class DeleteCompanyCommand : IRequest
    {
        public int CompanyId { get; set; }
    }
}

using MediatR;

namespace CompanyProject.Application.Companies.CreateCompany
{
    public class CreateCompanyCommand : IRequest<int>
    {
        public string CompanyName { get; set; } = string.Empty;
    }
}

using MediatR;
using CompanyProject.Domain.Entities;
using System.Collections.Generic;

namespace CompanyProject.Application.Companies.GetCompanies
{
    public class GetCompaniesQuery : IRequest<List<Company>>
    {
    }
}

using MediatR;
using CompanyProject.Domain.Entities;
using System.Collections.Generic;

namespace CompanyProject.Application.History.GetCompanyAudit
{
    public class GetCompanyAuditQuery: IRequest<List<ChangeHistory>>
    {
    }
}

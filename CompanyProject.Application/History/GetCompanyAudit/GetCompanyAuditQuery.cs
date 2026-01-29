using MediatR;
using CompanyProject.Domain.Entities;
using System.Collections.Generic;
using CompanyProject.Application.History.Dtos;


namespace CompanyProject.Application.History.GetCompanyAudit
{
    public class GetCompanyAuditQuery: IRequest<List<ChangeHistoryDto>>
    {
    }
}

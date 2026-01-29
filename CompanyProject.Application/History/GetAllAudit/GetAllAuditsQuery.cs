using CompanyProject.Application.History.Dtos;
using CompanyProject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProject.Application.History.GetAllAudit
{
    public class GetAllAuditsQuery : IRequest<List<ChangeHistoryDto>>
    {
    }
}

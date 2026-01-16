using MediatR;
using CompanyProject.Domain.Entities;
using System.Collections.Generic;

namespace CompanyProject.Application.Projects.GetProjects
{
    public class GetProjectsQuery : IRequest<List<Project>>
    {
    }
}

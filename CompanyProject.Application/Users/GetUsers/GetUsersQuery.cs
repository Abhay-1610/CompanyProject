using CompanyProject.Domain.Entities;
using CompanyProject.Infrastructure.Data;
using MediatR;
using System.Collections.Generic;

namespace CompanyProject.Application.Users.GetUsers
{
    public class GetUsersQuery : IRequest<List<ApplicationUser>>
    {
    }
}

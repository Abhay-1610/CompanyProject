using MediatR;
using System.Collections.Generic;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Application.Users.GetUsers
{
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
    }
}

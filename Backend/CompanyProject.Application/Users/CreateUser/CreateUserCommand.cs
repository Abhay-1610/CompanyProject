using CompanyProject.Application.Common.Dtos;
using MediatR;

namespace CompanyProject.Application.Users.CreateUser
{
    public class CreateUserCommand : IRequest<UserDto>

    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Used ONLY by SuperAdmin when creating CompanyAdmin
        public int CompanyId { get; set; }

        // Expected values: "CompanyAdmin" or "User"
        public string Role { get; set; } = string.Empty;
    }
}



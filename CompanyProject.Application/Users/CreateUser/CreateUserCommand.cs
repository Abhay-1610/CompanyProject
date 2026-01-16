using MediatR;

namespace CompanyProject.Application.Users.CreateUser
{
    public class CreateUserCommand : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? CompanyId { get; set; }   // null for SuperAdmin
        public string Role { get; set; } = string.Empty;
    }
}


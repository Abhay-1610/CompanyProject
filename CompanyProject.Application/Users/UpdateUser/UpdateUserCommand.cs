using MediatR;

namespace CompanyProject.Application.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

using MediatR;

namespace CompanyProject.Application.Users.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public string UserId { get; set; } = string.Empty;
    }
}

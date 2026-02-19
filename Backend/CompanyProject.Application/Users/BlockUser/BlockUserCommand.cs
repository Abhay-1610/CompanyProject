using MediatR;

namespace CompanyProject.Application.Users.BlockUser
{
    // Block or unblock a user
    public class BlockUserCommand : IRequest
    {
        public string UserId { get; set; } = string.Empty;
    }
}

using MediatR;

namespace CompanyProject.Application.Users.BlockUser
{
    // Block or unblock a user
    public class BlockUserCommand : IRequest
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }   // true = block, false = unblock
    }
}

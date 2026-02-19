using CompanyProject.Application.Common.Dtos;
using MediatR;

namespace CompanyProject.Application.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}

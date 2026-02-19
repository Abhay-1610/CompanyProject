using CompanyProject.Application.Common.Dtos;
using CompanyProject.Application.Interfaces;
using MediatR;

namespace CompanyProject.Application.Users.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<UserDto> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {

            var emailExists = await _userRepository.EmailExistsAsync(request.Email);
            if (emailExists)
                throw new UnauthorizedAccessException("User with this email already exists.");
            // 🔐 CompanyAdmin restriction
            if (!_currentUser.IsSuperAdmin && request.Role != "CompanyUser")
                throw new UnauthorizedAccessException();

            var user = await _userRepository.AddAsync(
                request.Email,
                request.Password,
                _currentUser.IsSuperAdmin
                    ? request.CompanyId
                    : _currentUser.CompanyId!.Value,
                request.Role
            );

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                Role = request.Role,
                CompanyId = user.CompanyId,
                IsBlocked = false
            };
        }
    }
}

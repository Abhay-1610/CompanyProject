using CompanyProject.Application.Interfaces;

using MediatR;

namespace CompanyProject.Application.Users.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, string>
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

        public async Task<string> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            // Only SuperAdmin can create Company Admin
            // Company Admin can create Company Users (handled by role checks later)

            await _userRepository.AddAsync(
                request.Email,
                request.Password,
                request.CompanyId ?? 0,
                request.Role
);

            return request.Email;

        }
    }
}

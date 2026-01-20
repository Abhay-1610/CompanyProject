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
            // 🔒 SuperAdmin → can create ONLY CompanyAdmin
            if (_currentUser.IsSuperAdmin)
            {
                await _userRepository.AddAsync(
                    request.Email,
                    request.Password,
                    request.CompanyId.Value,
                    request.Role
                );

                return request.Email;
            }

            // 🔒 CompanyAdmin → can create ONLY normal users (same company)
            if (request.Role != "CompanyUser")
                throw new UnauthorizedAccessException();

            var companyId = _currentUser.CompanyId
                ?? throw new UnauthorizedAccessException();

            await _userRepository.AddAsync(
                request.Email,
                request.Password,
                companyId,
                request.Role
            );

            return request.Email;
        }
    }
}

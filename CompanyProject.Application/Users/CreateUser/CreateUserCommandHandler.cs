using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using CompanyProject.Infrastructure.Data;
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

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                CompanyId = request.CompanyId
            };

            await _userRepository.AddAsync(user, request.Password, request.Role);

            return user.Id; // Identity generates this
        }
    }
}

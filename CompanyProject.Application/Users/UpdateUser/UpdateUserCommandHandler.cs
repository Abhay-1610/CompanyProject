using MediatR;
using CompanyProject.Application.Interfaces;
using CompanyProject.Application.Common.Dtos;

namespace CompanyProject.Application.Users.UpdateUser
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand , UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<UserDto> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            var emailExists = await _userRepository.EmailExistsAsync(request.Email, request.UserId);
            if (emailExists)
                throw new UnauthorizedAccessException("User with this email already exists.");

            // 🔒 SuperAdmin: can update ONLY CompanyAdmins
            if (_currentUser.IsSuperAdmin)
            {
                if (user == null)
                    throw new UnauthorizedAccessException();

                // CompanyAdmin always has a CompanyId
                if (user.CompanyId == null)
                    throw new UnauthorizedAccessException();

                await _userRepository.UpdateAsync(
                    request.UserId,
                    request.Email);

                return new UserDto
                {
                    CompanyId = user.CompanyId,
                    Email = request.Email,
                    Id = user.Id,
                    Role = request.Role,
                    IsBlocked = user.IsBlocked
                };
            }

            await _userRepository.UpdateAsync(
                request.UserId,
                request.Email);

            return new UserDto
            {
                CompanyId = user.CompanyId,
                Email = user.Email,
                Id = user.Id,
                Role = request.Role,
                IsBlocked = user.IsBlocked
            };
        }
    }
}

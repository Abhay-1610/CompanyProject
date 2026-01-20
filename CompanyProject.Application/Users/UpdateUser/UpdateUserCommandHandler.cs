using MediatR;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Application.Users.UpdateUser
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand>
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

        public async Task Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            // 🔒 SuperAdmin: can update ONLY CompanyAdmins
            if (_currentUser.IsSuperAdmin)
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                    throw new UnauthorizedAccessException();

                // CompanyAdmin always has a CompanyId
                if (user.CompanyId == null)
                    throw new UnauthorizedAccessException();

                await _userRepository.UpdateAsync(
                    request.UserId,
                    request.Email);

                return;
            }

            // 🔒 CompanyAdmin: can update ONLY users of own company
            var companyId = _currentUser.CompanyId
                ?? throw new UnauthorizedAccessException();

            var targetUser = await _userRepository.GetByIdAsync(request.UserId);
            if (targetUser == null || targetUser.CompanyId != companyId)
                throw new UnauthorizedAccessException();

            await _userRepository.UpdateAsync(
                request.UserId,
                request.Email);
        }
    }
}

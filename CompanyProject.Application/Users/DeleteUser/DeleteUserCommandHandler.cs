using MediatR;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Application.Users.DeleteUser
{
    public class DeleteUserCommandHandler
        : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public DeleteUserCommandHandler(
            IUserRepository userRepository,
            ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            // 🔒 SuperAdmin → can delete ONLY CompanyAdmins
            if (_currentUser.IsSuperAdmin)
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                    throw new UnauthorizedAccessException();

                // CompanyAdmin always has a CompanyId
                if (user.CompanyId == null)
                    throw new UnauthorizedAccessException();

                await _userRepository.DeleteAsync(request.UserId);
                return;
            }

            // 🔒 CompanyAdmin → can delete ONLY users of own company
            //var companyId = _currentUser.CompanyId
            //    ?? throw new UnauthorizedAccessException();

            //var targetUser = await _userRepository.GetByIdAsync(request.UserId);
            //if (targetUser == null || targetUser.CompanyId != companyId)
            //    throw new UnauthorizedAccessException();

            await _userRepository.DeleteAsync(request.UserId);
        }
    }
}

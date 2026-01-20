using MediatR;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Application.Users.BlockUser
{
    public class BlockUserCommandHandler
        : IRequestHandler<BlockUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public BlockUserCommandHandler(
            IUserRepository userRepository,
            ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task Handle(
            BlockUserCommand request,
            CancellationToken cancellationToken)
        {
            // 🔒 CompanyAdmin only
            if (_currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException();

            var companyId = _currentUser.CompanyId
                ?? throw new UnauthorizedAccessException();

            var targetUser = await _userRepository.GetByIdAsync(request.UserId);
            if (targetUser == null || targetUser.CompanyId != companyId)
                throw new UnauthorizedAccessException();

            if (request.IsBlocked)
            {
                await _userRepository.BlockAsync(request.UserId);
            }
            else
            {
                await _userRepository.UnblockAsync(request.UserId);
            }
        }
    }
}

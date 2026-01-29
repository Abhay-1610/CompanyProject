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
            await _userRepository.ToggleBlockAsync(request.UserId);
        }


    }
}

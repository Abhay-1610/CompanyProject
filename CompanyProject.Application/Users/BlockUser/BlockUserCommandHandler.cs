using MediatR;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Application.Users.BlockUser
{
    public class BlockUserCommandHandler
        : IRequestHandler<BlockUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public BlockUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(
            BlockUserCommand request,
            CancellationToken cancellationToken)
        {
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

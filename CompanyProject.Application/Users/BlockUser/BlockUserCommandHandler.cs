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
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
                throw new Exception("User not found");

            
            user.LockoutEnabled = true;
            user.LockoutEnd = request.IsBlocked ? DateTimeOffset.MaxValue: null;  

            await _userRepository.UpdateAsync(user);
        }
    }
}

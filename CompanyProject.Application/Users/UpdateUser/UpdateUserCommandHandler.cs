using MediatR;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Application.Users.UpdateUser
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            await _userRepository.UpdateAsync(
                request.UserId,
                request.Email);
        }
    }
}

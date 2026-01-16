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
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
                throw new Exception("User not found");

            user.Email = request.Email;
            user.UserName = request.Email;

            await _userRepository.UpdateAsync(user);
        }
    }
}

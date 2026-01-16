using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using CompanyProject.Infrastructure.Data;
using MediatR;

namespace CompanyProject.Application.Users.GetUsers
{
    public class GetUsersQueryHandler
        : IRequestHandler<GetUsersQuery, List<ApplicationUser>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public GetUsersQueryHandler(
            IUserRepository userRepository,
            ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<List<ApplicationUser>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken)
        {
            return await _userRepository
                .GetByCompanyIdAsync(_currentUser.CompanyId);
        }
    }
}

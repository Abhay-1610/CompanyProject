using CompanyProject.Application.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace CompanyProject.Application.Users.GetUsers
{
    public class GetUsersQueryHandler
        : IRequestHandler<GetUsersQuery, List<UserDto>>
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

        public async Task<List<UserDto>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken)
        {
            return await _userRepository
                .GetByCompanyIdAsync(_currentUser.CompanyId);
        }
    }
}

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
            // 🔓 SuperAdmin → company comes from request
            if (_currentUser.IsSuperAdmin)
            {
                return await _userRepository.GetByCompanyIdAsync(request.CompanyId.Value);
            }

            // 🔒 CompanyAdmin → company comes from token
            var companyId = _currentUser.CompanyId
                ?? throw new UnauthorizedAccessException();

            return await _userRepository
                .GetByCompanyIdAsync(companyId);
        }
    }
}
 
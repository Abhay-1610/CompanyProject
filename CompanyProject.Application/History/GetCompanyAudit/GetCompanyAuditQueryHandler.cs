using MediatR;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using System.Collections.Generic;

namespace CompanyProject.Application.History.GetCompanyAudit
{
    public class GetCompanyAuditQueryHandler
        : IRequestHandler<GetCompanyAuditQuery, List<ChangeHistory>>
    {
        private readonly IChangeHistoryRepository _repository;
        private readonly ICurrentUser _currentUser;

        public GetCompanyAuditQueryHandler(
            IChangeHistoryRepository repository,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<List<ChangeHistory>> Handle(
            GetCompanyAuditQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository
                .GetByCompanyIdAsync(_currentUser.CompanyId);
        }
    }
}

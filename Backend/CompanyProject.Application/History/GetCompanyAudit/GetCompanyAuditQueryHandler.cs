using MediatR;
using CompanyProject.Application.Interfaces;
using CompanyProject.Application.History.Dtos;
using System.Linq;

namespace CompanyProject.Application.History.GetCompanyAudit
{
    public class GetCompanyAuditQueryHandler
        : IRequestHandler<GetCompanyAuditQuery, List<ChangeHistoryDto>>
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

        public async Task<List<ChangeHistoryDto>> Handle(
            GetCompanyAuditQuery request,
            CancellationToken cancellationToken)
        {
            var history = await _repository.GetByCompanyIdAsync(
                _currentUser.CompanyId ?? throw new UnauthorizedAccessException()
            );

            return history.Select(h => new ChangeHistoryDto
            {
                ChangeId = h.ChangeId,

                CompanyId = h.CompanyId,
                CompanyName = h.CompanyName,

                ProjectId = h.ProjectId,
                ProjectName = h.ProjectName,

                ChangeType = h.ChangeType,
                OldData = h.OldData,
                NewData = h.NewData,

                ChangedBy = h.ChangedBy,
                ChangedByEmail = h.ChangedByEmail, // replace later if you map users

                ChangedAt = h.ChangedAt
            }).ToList();

        }
    }
}

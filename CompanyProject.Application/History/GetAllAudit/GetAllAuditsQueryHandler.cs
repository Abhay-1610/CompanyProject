using MediatR;
using CompanyProject.Application.History.Dtos;
using System.Linq;

namespace CompanyProject.Application.History.GetAllAudit
{
    public class GetAllAuditsQueryHandler
        : IRequestHandler<GetAllAuditsQuery, List<ChangeHistoryDto>>
    {
        private readonly IChangeHistoryRepository _repository;

        public GetAllAuditsQueryHandler(IChangeHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ChangeHistoryDto>> Handle(
            GetAllAuditsQuery request,
            CancellationToken cancellationToken)
        {
            var history = await _repository.GetAllHistory();

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
                ChangedByEmail = h.ChangedByEmail,

                ChangedAt = h.ChangedAt
            }).ToList();

        }
    }
}

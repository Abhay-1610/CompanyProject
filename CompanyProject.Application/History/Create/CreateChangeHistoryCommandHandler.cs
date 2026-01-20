using MediatR;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;

namespace CompanyProject.Application.History.Create
{
    public class CreateChangeHistoryCommandHandler
        : IRequestHandler<CreateChangeHistoryCommand>
    {
        private readonly IChangeHistoryRepository _repository;
        private readonly ICurrentUser _currentUser;
        private readonly IRealtimeNotifier _realtimeNotifier;

        public CreateChangeHistoryCommandHandler(
            IChangeHistoryRepository repository,
            ICurrentUser currentUser,
            IRealtimeNotifier realtimeNotifier)
        {
            _repository = repository;
            _currentUser = currentUser;
            _realtimeNotifier = realtimeNotifier;
        }

        public async Task Handle(
            CreateChangeHistoryCommand request,
            CancellationToken cancellationToken)
        {
            var history = new ChangeHistory
            {
                CompanyId = _currentUser.CompanyId ?? throw new UnauthorizedAccessException(),
                ProjectId = request.ProjectId,
                ChangeType = request.ChangeType,
                OldData = request.OldData,
                NewData = request.NewData,
                ChangedBy = _currentUser.UserId,
                ChangedAt = DateTime.UtcNow
            };

            // 1️⃣ Save audit record
            await _repository.AddAsync(history);

            // 2️⃣ Notify company users in real-time
            await _realtimeNotifier.NotifyCompanyAsync(
                _currentUser.CompanyId ?? throw new UnauthorizedAccessException(),
                "AuditChanged",
                request.ProjectId);
        }
    }
}

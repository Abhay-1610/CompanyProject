using MediatR;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using CompanyProject.Application.History.Dtos;

namespace CompanyProject.Application.History.Create
{
    public class CreateChangeHistoryCommandHandler
        : IRequestHandler<CreateChangeHistoryCommand>
    {
        private readonly IChangeHistoryRepository _repository;
        private readonly ICurrentUser _currentUser;
        private readonly IRealtimeNotifier _realtimeNotifier;
        private readonly ICompanyRepository _companyRepository;


        public CreateChangeHistoryCommandHandler(
            IChangeHistoryRepository repository,
            ICurrentUser currentUser,
            IRealtimeNotifier realtimeNotifier,
            ICompanyRepository companyRepository)
        {
            _repository = repository;
            _currentUser = currentUser;
            _realtimeNotifier = realtimeNotifier;
            _companyRepository = companyRepository;
        }

        public async Task Handle(
            CreateChangeHistoryCommand request,
            CancellationToken cancellationToken)
        {

            string? companyName = request.CompanyName;

            if (companyName == "" && request.companyId.HasValue)
            {
                var company = await _companyRepository.GetByIdAsync(request.companyId.Value);
                companyName = company?.CompanyName;
            }
            // 1️⃣ Create audit entity
            var history = new ChangeHistory
            {
                CompanyId = request.companyId,
                CompanyName = companyName ?? "System",
                ProjectId = request.ProjectId,
                ProjectName = request.ProjectName,
                ChangeType = request.ChangeType,
                OldData = request.OldData,
                NewData = request.NewData,
                ChangedBy = _currentUser.UserId,
                ChangedByEmail = _currentUser.Email,
                ChangedAt = DateTime.UtcNow
            };

            //  Save audit record
            await _repository.AddAsync(history);

            //  Do NOT notify for SuperAdmin
            if (_currentUser.IsSuperAdmin)
                return;

            //  Map to DTO (UI-ready payload)
            var dto = new ChangeHistoryDto
            {
                ChangeId = history.ChangeId,
                CompanyId = history.CompanyId,
                CompanyName = history.CompanyName, 
                ProjectId = history.ProjectId,
                ProjectName = history.ProjectName,
                ChangeType = history.ChangeType,
                OldData = history.OldData,
                NewData = history.NewData,
                ChangedBy = history.ChangedBy,
                ChangedByEmail = history.ChangedByEmail,
                ChangedAt = history.ChangedAt
            };

            await _realtimeNotifier.NotifyCompanyAsync(
                _currentUser.CompanyId ?? throw new UnauthorizedAccessException(),"AuditChanged", dto);
        }
    }
}




using CompanyProject.Domain.Entities;

namespace CompanyProject.Application.History
{
    public interface IChangeHistoryRepository
    {
        Task AddAsync(ChangeHistory history);

        Task<List<ChangeHistory>> GetByCompanyIdAsync(int companyId);
    }
}

using CompanyProject.Application.History;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using CompanyProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Infrastructure.Repositories
{
    public class ChangeHistoryRepository : IChangeHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ChangeHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE AUDIT ENTRY
        public async Task AddAsync(ChangeHistory history)
        {
            _context.ChangeHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        // GET AUDIT TRAIL COMPANY-WISE
        public async Task<List<ChangeHistory>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.ChangeHistories
                .Where(h => h.CompanyId == companyId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }
    }
}

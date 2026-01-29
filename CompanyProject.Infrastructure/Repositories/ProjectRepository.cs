using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using CompanyProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create project
        public async Task AddAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        // Get project by id
        public async Task<Project?> GetByIdAsync(int projectId)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId);
        }

        // Get all projects for a company
        public async Task<List<Project>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Projects.Where(p => p.CompanyId == companyId).ToListAsync();
        }

        // Update project
        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        // Delete project
        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ProjectNameExistsAsync(
             string projectName,
             int companyId,
             int? excludeProjectId = null)
        {
            return await _context.Projects.AnyAsync(p =>
                p.CompanyId == companyId &&
                p.ProjectName == projectName &&
                (!excludeProjectId.HasValue || p.ProjectId != excludeProjectId)
            );
        }


    }
}

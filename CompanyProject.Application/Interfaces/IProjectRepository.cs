using CompanyProject.Domain.Entities;

namespace CompanyProject.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);               // Create Project

        Task<Project?> GetByIdAsync(int projectId);   // Get Project By Id

        Task<List<Project>> GetByCompanyIdAsync(int companyId); // Get Projects of a Company

        Task UpdateAsync(Project project);             // Update Project

        Task DeleteAsync(Project project);             // Delete Project

        Task<bool> ProjectNameExistsAsync(string projectName, int companyId,int? excludeProjectId = null);

    }
}

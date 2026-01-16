using CompanyProject.Domain.Entities;

namespace CompanyProject.Application.Interfaces
{
    public interface ICompanyRepository
    {
        Task AddAsync(Company company);              //Create Company

        Task<Company?> GetByIdAsync(int companyId);  // Get Company By Id

        Task<List<Company>> GetAllAsync();           // Get All Companies

        Task UpdateAsync(Company company);           // Update Company

        Task DeleteAsync(Company company);           //Delete Company
    }
}

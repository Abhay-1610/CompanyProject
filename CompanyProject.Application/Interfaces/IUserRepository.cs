using CompanyProject.Infrastructure.Data;

    public interface IUserRepository
    {
        Task AddAsync(ApplicationUser user ,string password,string role);           // Create User

        Task<ApplicationUser?> GetByIdAsync(string userId); // Get User By Id

        Task<List<ApplicationUser>> GetByCompanyIdAsync(int companyId); // Get Users of a Company

        Task UpdateAsync(ApplicationUser user);         // Update User

        Task DeleteAsync(ApplicationUser user);         // Delete User
    }
}

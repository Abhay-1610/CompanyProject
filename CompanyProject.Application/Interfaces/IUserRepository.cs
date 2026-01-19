using CompanyProject.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(string email, string password, int companyId, string role);

    Task<UserDto?> GetByIdAsync(string userId);

    Task<List<UserDto>> GetByCompanyIdAsync(int companyId);

    Task UpdateAsync(string userId, string email);

    Task DeleteAsync(string userId);

    Task BlockAsync(string userId);
    Task UnblockAsync(string userId);
}

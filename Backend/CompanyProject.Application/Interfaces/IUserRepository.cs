using CompanyProject.Application.Common.Dtos;

public interface IUserRepository
{
    Task<UserDto> AddAsync(
     string email,
     string password,
     int companyId,
     string role
 );
    Task<bool> EmailExistsAsync(string email, string userId);
    Task<bool> EmailExistsAsync(string email);

    Task<bool> IsUserBlockedAsync(string userId);
    Task ToggleBlockAsync(string userId);

    Task<UserDto?> GetByIdAsync(string userId);

    Task<List<UserDto>> GetByCompanyIdAsync(int companyId);

    Task UpdateAsync(string userId, string email);

    Task DeleteAsync(string userId);

    Task BlockAsync(string userId);
    Task UnblockAsync(string userId);
}

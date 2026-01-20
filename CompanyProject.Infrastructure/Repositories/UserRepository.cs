using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using CompanyProject.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

  
        public async Task AddAsync(string email,string password,int companyId,string role)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                CompanyId = companyId
            };

            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<UserDto?> GetByIdAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                CompanyId = user.CompanyId,
                IsBlocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow
            };
        }

        public async Task<List<UserDto>> GetByCompanyIdAsync(int companyId)
        {
            return await _userManager.Users.Where(u => u.CompanyId == companyId).Select(u => 
            new UserDto
                {
                    Id = u.Id,
                    Email = u.Email ?? string.Empty,
                    CompanyId = u.CompanyId,
                    IsBlocked = u.LockoutEnd.HasValue &&
                                u.LockoutEnd > DateTimeOffset.UtcNow
                }).ToListAsync();
        }

        
        public async Task UpdateAsync(string userId, string email)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            user.Email = email;
            user.UserName = email;

            await _userManager.UpdateAsync(user);
        }

        
        public async Task DeleteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            await _userManager.DeleteAsync(user);
        }

        
        public async Task BlockAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            await _userManager.SetLockoutEndDateAsync(
                user,
                DateTimeOffset.MaxValue);
        }

        
        public async Task UnblockAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            await _userManager.SetLockoutEndDateAsync(user, null);
        }
    }
}

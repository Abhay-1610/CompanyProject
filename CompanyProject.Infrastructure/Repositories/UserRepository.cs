using CompanyProject.Application.Common.Dtos;
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

        public async Task<bool> EmailExistsAsync(string email, string userId)
        {
            return await _userManager.Users.AnyAsync(u =>
                u.Email == email &&
                u.Id != userId
            );
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(u =>
                u.Email == email
            );
        }


        public async Task<bool> IsUserBlockedAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new UnauthorizedAccessException(
                    "Please contact your admin to make your account");

            return user.LockoutEnd.HasValue &&
                   user.LockoutEnd.Value > DateTimeOffset.UtcNow;
        }

        public async Task ToggleBlockAsync(string userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            if (user.LockoutEnd.HasValue &&
                user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                // Unblock
                user.LockoutEnd = null;
            }
            else
            {
                // Block
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
            }

            await _userManager.UpdateAsync(user);
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
            var users = await _userManager.Users
                .Where(u => u.CompanyId == companyId)
                .ToListAsync();

            var result = new List<UserDto>();

            foreach (var u in users)
            {
                var role = (await _userManager.GetRolesAsync(u)).FirstOrDefault();

                result.Add(new UserDto
                {
                    Id = u.Id,
                    Email = u.Email ?? string.Empty,
                    CompanyId = u.CompanyId,
                    Role = role,
                    IsBlocked = u.LockoutEnd.HasValue &&
                                u.LockoutEnd > DateTimeOffset.UtcNow
                });
            }

            return result;
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

        async Task<UserDto> IUserRepository.AddAsync(
     string email,
     string password,
     int companyId,
     string role)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                CompanyId = companyId
            };

            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, role);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Role = role,
                CompanyId = user.CompanyId,
                IsBlocked = false
            };
        }

    }
}

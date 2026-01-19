using Microsoft.AspNetCore.Identity;

namespace CompanyProject.Infrastructure.Security
{
    public class RoleSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleSeeder(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // Create SuperAdmin role
            if (!await _roleManager.RoleExistsAsync(RoleConstants.SuperAdmin))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(RoleConstants.SuperAdmin));
            }

            // Create CompanyAdmin role
            if (!await _roleManager.RoleExistsAsync(RoleConstants.CompanyAdmin))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(RoleConstants.CompanyAdmin));
            }

            // Create CompanyUser role
            if (!await _roleManager.RoleExistsAsync(RoleConstants.CompanyUser))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(RoleConstants.CompanyUser));
            }
        }
    }
}

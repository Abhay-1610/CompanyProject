using Azure.Core;
using CompanyProject.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace CompanyProject.Infrastructure.Security
{
    public class RoleSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoleSeeder(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            // Create SuperAdmin role
            if (!await _roleManager.RoleExistsAsync(RoleConstants.SuperAdmin))
            {
                var user = new ApplicationUser
                {
                    UserName = "superAdmin@gmail.com",
                    Email = "superAdmin@gmail.com",
                    CompanyId = null // NULL = SuperAdmin
                };

                var result = await _userManager.CreateAsync(user, "Pass@123");

                // 4️⃣ Assign role

                await _roleManager.CreateAsync(new IdentityRole(RoleConstants.SuperAdmin));

                await _userManager.AddToRoleAsync(user, RoleConstants.SuperAdmin);                
               
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

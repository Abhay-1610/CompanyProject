using CompanyProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CompanyProject.Infrastructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int? CompanyId { get; set; }     // FK (nullable for SuperAdmin)
        public Company? Company { get; set; }
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CompanyProject.Application.Interfaces;

namespace CompanyProject.Infrastructure.Security
{
    public class CurrentUserService : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null) return string.Empty;

                return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            }
        }

        // null => SuperAdmin (not company-scoped)
        public int? CompanyId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null) return null;

                var companyClaim = user.FindFirst("companyId");
                if (companyClaim == null) return null;

                return int.Parse(companyClaim.Value);
            }
        }

        public string? CompanyName
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null) return null;

                return user.FindFirst("companyName")?.Value;
            }
        }

        public string Role
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null) return string.Empty;

                return user.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            }
        }

        public bool IsSuperAdmin
        {
            get
            {
                return Role == "SuperAdmin";
            }
        }

        public string? Email =>                    
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.Email);
    }

}

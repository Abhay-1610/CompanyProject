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

                return user.FindFirstValue(ClaimTypes.NameIdentifier)?? string.Empty;
            }
        }

        // 0 means SuperAdmin (no company)
        public int CompanyId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null) return 0;

                var companyClaim = user.FindFirst("CompanyId");

                if (companyClaim == null) return 0;

                return int.Parse(companyClaim.Value);
            }
        }

        public string Role
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null) return string.Empty;

                return user.FindFirstValue(ClaimTypes.Role)?? string.Empty;
            }
        }

        public bool IsBlocked
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null) return false;

                var lockoutClaim = user.FindFirst("LockoutEnd");

                if (lockoutClaim == null) return false;

                var lockoutEnd = DateTimeOffset.Parse(lockoutClaim.Value);

                return lockoutEnd > DateTimeOffset.UtcNow;
            }
        }
    }
}



using CompanyProject.Application.Common.Dtos;
using System.Security.Claims;

namespace CompanyProject.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(UserDto user,string role);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

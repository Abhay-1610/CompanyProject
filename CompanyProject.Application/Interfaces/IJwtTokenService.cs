

namespace CompanyProject.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(UserDto user, IList<string> roles);
}

using CompanyProject.Application.Common.Dtos;
using CompanyProject.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CompanyProject.Infrastructure.Security;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;
    private readonly ICompanyRepository _companyRepository;

    public JwtTokenService(IConfiguration config, ICompanyRepository companyRepository)
    {
        _config = config;
        _companyRepository = companyRepository;
    }

    public string GenerateToken(UserDto user, string role)
    {
        string companyName = string.Empty;

        if (user.CompanyId.HasValue)
        {
            var company = _companyRepository
                .GetByIdAsync(user.CompanyId.Value)
                .Result;

            companyName = company?.CompanyName ?? string.Empty;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role),

            new Claim("userId", user.Id),
            new Claim("email", user.Email),
            new Claim("role", role),
            new Claim("companyId", user.CompanyId?.ToString() ?? string.Empty),
            new Claim("companyName", companyName)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!)
        );

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiryMinutes"]!)),
            signingCredentials: new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        // key used to sign the JWT
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!)
        );

        // rules to read the token
        var rules = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, // token must be ours
            IssuerSigningKey = key,

            ValidateIssuer = false,   // not using issuer
            ValidateAudience = false, // not using audience

            ValidateLifetime = false  // allow expired token
        };

        var handler = new JwtSecurityTokenHandler();

        // read token and get claims
        var principal = handler.ValidateToken(token, rules, out _);

        return principal;
    }

}

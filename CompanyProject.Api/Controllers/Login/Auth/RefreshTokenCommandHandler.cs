using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CompanyProject.Application.Common.Dtos;
using CompanyProject.Application.Common.Security;
using CompanyProject.Application.Interfaces;
using CompanyProject.Infrastructure.Data;

namespace CompanyProject.Api.Controllers.Login.Auth;

public sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtService;
    private readonly IConfiguration _config;

    public RefreshTokenCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtService,
        IConfiguration config)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _config = config;
    }

    public async Task<AuthResponseDto> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        // read user info from expired access token
        var claims =
            _jwtService.GetPrincipalFromExpiredToken(
                request.AccessToken
            );

        var userId =
            claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            throw new UnauthorizedAccessException();

        // get user from database
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            CompanyId = user.CompanyId
        };

        var newAccessToken =
            _jwtService.GenerateToken(userDto, role!);

        // create new refresh token
        var newRefreshToken =
            RefreshTokenGenerator.Generate();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime =
            DateTime.UtcNow.AddDays(
                int.Parse(_config["Jwt:RefreshTokenDays"]!)
            );

        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}

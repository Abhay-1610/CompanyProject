using MediatR;
using Microsoft.AspNetCore.Identity;
using CompanyProject.Application.Interfaces;
using CompanyProject.Infrastructure.Data;
using CompanyProject.Application.Common.Dtos;
using CompanyProject.Application.Common.Security;

namespace CompanyProject.Api.Login.Auth;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IConfiguration _config;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService,
        IConfiguration config)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _config = config;
    }

    public async Task<AuthResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid Email");

        var validPassword =
            await _userManager.CheckPasswordAsync(user, request.Password);

        if (!validPassword)
            throw new UnauthorizedAccessException("Invalid Password");


        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            CompanyId = user.CompanyId
        };

        // access token
        var accessToken =  _jwtTokenService.GenerateToken(userDto, role!);

        // refresh token
        var refreshToken = RefreshTokenGenerator.Generate();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime =  DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenDays"]!));

        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}

using MediatR;
using Microsoft.AspNetCore.Identity;
using CompanyProject.Application.Interfaces;
using CompanyProject.Domain.Entities;
using CompanyProject.Infrastructure.Data;

namespace CompanyProject.Api.Login.Auth;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<string> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user.LockoutEnd.HasValue &&
          user.LockoutEnd.Value > DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedAccessException("User is blocked");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(
            user, request.Password, false);

        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid credentials");

        var roles = await _userManager.GetRolesAsync(user);

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            CompanyId = user.CompanyId,
           IsBlocked = user.LockoutEnd.HasValue
            && user.LockoutEnd.Value > DateTimeOffset.UtcNow

        };

        return _jwtTokenService.GenerateToken(userDto, roles);
    }
}

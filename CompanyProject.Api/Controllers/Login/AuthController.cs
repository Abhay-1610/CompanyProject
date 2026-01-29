using CompanyProject.Api.Controllers.Login.Auth;
using CompanyProject.Api.Login.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompanyProject.Api.Controllers.Login;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var result = await _mediator.Send(
            new LoginCommand(request.Email, request.Password)
        );

        return Ok(result);
    }

    // REFRESH TOKEN
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

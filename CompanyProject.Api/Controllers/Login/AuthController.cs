

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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var token = await _mediator.Send(new LoginCommand(request.Email, request.Password));

        return Ok(new { accessToken = token });
    }
}

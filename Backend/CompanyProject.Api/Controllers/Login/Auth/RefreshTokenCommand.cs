using MediatR;
using CompanyProject.Application.Common.Dtos;

namespace CompanyProject.Api.Controllers.Login.Auth;

public sealed class RefreshTokenCommand
    : IRequest<AuthResponseDto>
{
    // expired access token
    public string AccessToken { get; set; } = string.Empty;

    // refresh token from client
    public string RefreshToken { get; set; } = string.Empty;
}

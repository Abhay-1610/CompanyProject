using MediatR;
using CompanyProject.Application.Common.Dtos;

namespace CompanyProject.Api.Login.Auth;

public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponseDto>;

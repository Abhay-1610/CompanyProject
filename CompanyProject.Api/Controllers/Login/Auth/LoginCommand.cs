using MediatR;

namespace CompanyProject.Api.Login.Auth;

public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<string>;


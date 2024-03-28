using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<ErrorOr<LoginResult>>;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Queries.Login;

public sealed record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<LoginResult>>;
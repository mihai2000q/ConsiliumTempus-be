using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Queries;

public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<LoginResult>>;
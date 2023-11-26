using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<RegisterResult>>;
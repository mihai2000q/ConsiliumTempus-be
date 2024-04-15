using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string? Role,
    DateOnly? DateOfBirth)
    : IRequest<ErrorOr<RegisterResult>>;
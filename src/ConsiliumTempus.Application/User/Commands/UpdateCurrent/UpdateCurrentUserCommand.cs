using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.UpdateCurrent;

public sealed record UpdateCurrentUserCommand(
    string FirstName,
    string LastName,
    string? Role,
    DateOnly? DateOfBirth)
    : IRequest<ErrorOr<UpdateCurrentUserResult>>;
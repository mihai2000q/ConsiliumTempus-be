using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.Update;

public sealed record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string? Role,
    DateOnly? DateOfBirth) 
    : IRequest<ErrorOr<UpdateUserResult>>;
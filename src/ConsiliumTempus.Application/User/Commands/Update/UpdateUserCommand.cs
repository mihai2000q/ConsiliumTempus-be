using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.Update;

public sealed record UpdateUserCommand(
    string FirstName,
    string LastName,
    string? Role,
    DateOnly? DateOfBirth) 
    : IRequest<ErrorOr<UpdateUserResult>>;
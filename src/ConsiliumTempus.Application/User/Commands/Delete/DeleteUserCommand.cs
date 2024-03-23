using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.Delete;

public sealed record DeleteUserCommand : IRequest<ErrorOr<DeleteUserResult>>;
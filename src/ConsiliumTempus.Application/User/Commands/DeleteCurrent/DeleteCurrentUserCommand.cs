using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.DeleteCurrent;

public sealed record DeleteCurrentUserCommand : IRequest<ErrorOr<DeleteCurrentUserResult>>;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.LeavePrivate;

public sealed record LeavePrivateProjectCommand(Guid Id) : IRequest<ErrorOr<LeavePrivateProjectResult>>;
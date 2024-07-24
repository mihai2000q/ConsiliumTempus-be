using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Leave;

public sealed record LeaveWorkspaceCommand(
    Guid Id)
    : IRequest<ErrorOr<LeaveWorkspaceResult>>;
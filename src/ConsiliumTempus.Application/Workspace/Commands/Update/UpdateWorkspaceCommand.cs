using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Update;

public sealed record UpdateWorkspaceCommand(
    Guid Id,
    string Name)
    : IRequest<ErrorOr<UpdateWorkspaceResult>>;
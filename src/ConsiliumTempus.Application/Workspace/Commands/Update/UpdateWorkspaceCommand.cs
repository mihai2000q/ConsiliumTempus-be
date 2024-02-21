using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Update;

public sealed record UpdateWorkspaceCommand(
    Guid Id,
    string Name,
    string Description)
    : IRequest<ErrorOr<UpdateWorkspaceResult>>;
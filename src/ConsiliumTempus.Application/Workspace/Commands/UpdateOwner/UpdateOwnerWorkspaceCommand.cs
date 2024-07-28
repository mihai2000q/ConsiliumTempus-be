using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;

public sealed record UpdateOwnerWorkspaceCommand(
    Guid Id,
    Guid OwnerId)
    : IRequest<ErrorOr<UpdateOwnerWorkspaceResult>>;
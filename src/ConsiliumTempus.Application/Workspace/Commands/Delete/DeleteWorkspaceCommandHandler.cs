using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Delete;

public sealed class DeleteWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<DeleteWorkspaceCommand, ErrorOr<DeleteWorkspaceResult>>
{
    public async Task<ErrorOr<DeleteWorkspaceResult>> Handle(DeleteWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspaceId = WorkspaceId.Create(command.Id);
        var workspace = await workspaceRepository.Get(workspaceId, cancellationToken);

        if (workspace is null) return Errors.Workspace.NotFound;

        workspaceRepository.Remove(workspace);

        return new DeleteWorkspaceResult();
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Update;

public sealed class UpdateWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<UpdateWorkspaceCommand, ErrorOr<UpdateWorkspaceResult>>
{
    public async Task<ErrorOr<UpdateWorkspaceResult>> Handle(UpdateWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.Get(WorkspaceId.Create(command.Id), cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        workspace.Update(
            Name.Create(command.Name),
            Description.Create(command.Description));

        return new UpdateWorkspaceResult();
    }
}
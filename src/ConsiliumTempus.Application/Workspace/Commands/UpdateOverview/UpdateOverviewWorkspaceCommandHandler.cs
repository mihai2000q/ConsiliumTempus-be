using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;

public sealed class UpdateOverviewWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<UpdateOverviewWorkspaceCommand, ErrorOr<UpdateOverviewWorkspaceResult>>
{
    public async Task<ErrorOr<UpdateOverviewWorkspaceResult>> Handle(UpdateOverviewWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.Get(WorkspaceId.Create(command.Id), cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;
        
        workspace.UpdateOverview(
            Description.Create(command.Description));

        return new UpdateOverviewWorkspaceResult();
    }
}
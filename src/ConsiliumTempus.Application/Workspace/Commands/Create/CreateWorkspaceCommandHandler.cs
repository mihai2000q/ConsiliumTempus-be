using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

public sealed class CreateWorkspaceCommandHandler(
    ISecurity security,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceResult>
{
    public async Task<CreateWorkspaceResult> Handle(CreateWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var user = await security.GetUserFromToken(command.Token, cancellationToken);

        var workspace = WorkspaceAggregate.Create(
            Name.Create(command.Name),
            Description.Create(command.Description));
        await workspaceRepository.Add(workspace, cancellationToken);

        var membership = Membership.Create(user, workspace, WorkspaceRole.Admin);
        workspace.AddUserMembership(membership);

        return new CreateWorkspaceResult(workspace);
    }
}
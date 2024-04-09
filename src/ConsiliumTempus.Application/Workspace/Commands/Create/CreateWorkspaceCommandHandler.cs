using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

public sealed class CreateWorkspaceCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<CreateWorkspaceCommand, ErrorOr<CreateWorkspaceResult>>
{
    public async Task<ErrorOr<CreateWorkspaceResult>> Handle(CreateWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        
        if (user is null) return Errors.User.NotFound;

        var workspace = WorkspaceAggregate.Create(
            Name.Create(command.Name),
            Description.Create(command.Description),
            user,
            IsUserWorkspace.Create(false));
        await workspaceRepository.Add(workspace, cancellationToken);

        var membership = Membership.Create(user, workspace, WorkspaceRole.Admin);
        workspace.AddUserMembership(membership);

        return new CreateWorkspaceResult();
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Workspace;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

public class CreateWorkspaceCommandHandler(
    ISecurity security,
    IWorkspaceRepository workspaceRepository,
    IWorkspaceRoleRepository workspaceRoleRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceResult>
{
    public async Task<CreateWorkspaceResult> Handle(CreateWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var user = await security.GetUserFromToken(command.Token);
        
        var workspace = WorkspaceAggregate.Create(
            command.Name,
            command.Description);

        var role = WorkspaceRole.Admin;
        workspaceRoleRepository.Attach(role);
        var membership = Membership.Create(user, workspace, role);
        workspace.AddUserMembership(membership);
        
        await workspaceRepository.Add(workspace);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateWorkspaceResult(workspace);
    }
}
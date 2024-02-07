using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Domain.Workspace;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

public class CreateWorkspaceCommandHandler(
    ISecurity security,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<CreateWorkspaceCommand, ErrorOr<CreateWorkspaceResult>>
{
    public async Task<ErrorOr<CreateWorkspaceResult>> Handle(CreateWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var user = await security.GetUserFromToken(command.Token);
        
        var workspace = WorkspaceAggregate.Create(
            command.Name,
            command.Description);
        workspace.AddUser(user);
        await workspaceRepository.Add(workspace);

        return new CreateWorkspaceResult(workspace);
    }
}
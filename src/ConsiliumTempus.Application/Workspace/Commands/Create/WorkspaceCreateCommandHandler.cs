using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Domain.Workspace;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

public class WorkspaceCreateCommandHandler(
    ISecurity security,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<WorkspaceCreateCommand, ErrorOr<WorkspaceCreateResult>>
{
    public async Task<ErrorOr<WorkspaceCreateResult>> Handle(WorkspaceCreateCommand command,
        CancellationToken cancellationToken)
    {
        var user = await security.GetUserFromToken(command.Token);

        if (user.IsError) return user.Errors;
        
        var workspace = WorkspaceAggregate.Create(
            command.Name,
            command.Description);
        workspace.AddUser(user.Value);
        await workspaceRepository.Add(workspace);

        return new WorkspaceCreateResult(workspace);
    }
}
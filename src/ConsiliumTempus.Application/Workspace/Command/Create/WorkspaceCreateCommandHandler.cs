using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.Workspace;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Command.Create;

public class WorkspaceCreateCommandHandler(
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<WorkspaceCreateCommand, ErrorOr<WorkspaceCreateResult>>
{
    public async Task<ErrorOr<WorkspaceCreateResult>> Handle(WorkspaceCreateCommand command,
        CancellationToken cancellationToken)
    {
        var workspace = WorkspaceAggregate.Create(
            command.Name,
            command.Description);

        await workspaceRepository.Add(workspace);

        return new WorkspaceCreateResult("It Worked");
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;

internal sealed class UpdateOverviewProjectTaskCommandHandler(
    IProjectTaskRepository projectTaskRepository,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<UpdateOverviewProjectTaskCommand, ErrorOr<UpdateOverviewProjectTaskResult>>
{
    public async Task<ErrorOr<UpdateOverviewProjectTaskResult>> Handle(UpdateOverviewProjectTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await projectTaskRepository.GetWithWorkspace(
            ProjectTaskId.Create(command.Id),
            cancellationToken);
        if (task is null) return Errors.ProjectTask.NotFound;

        var assignee = command.AssigneeId is not null
            ? await workspaceRepository.GetCollaborator(
                UserId.Create(command.AssigneeId.Value),
                task.Stage.Sprint.Project.Workspace.Id,
                cancellationToken)
            : null;
        if (command.AssigneeId is not null && assignee is null) return Errors.User.NotFound;

        task.UpdateOverview(
            Name.Create(command.Name),
            Description.Create(command.Description),
            assignee);
        task.Stage.Sprint.Project.RefreshActivity();

        return new UpdateOverviewProjectTaskResult();
    }
}
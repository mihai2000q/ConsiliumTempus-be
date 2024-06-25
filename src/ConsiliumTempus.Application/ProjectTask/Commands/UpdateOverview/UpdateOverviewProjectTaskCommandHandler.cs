using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;

public sealed class UpdateOverviewProjectTaskCommandHandler(
    IProjectTaskRepository projectTaskRepository,
    IUserRepository userRepository)
    : IRequestHandler<UpdateOverviewProjectTaskCommand, ErrorOr<UpdateOverviewProjectTaskResult>>
{
    public async Task<ErrorOr<UpdateOverviewProjectTaskResult>> Handle(UpdateOverviewProjectTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await projectTaskRepository.Get(
            ProjectTaskId.Create(command.Id),
            cancellationToken);
        if (task is null) return Errors.ProjectTask.NotFound;

        var assignee = command.AssigneeId is not null
            ? await userRepository.Get(UserId.Create(command.AssigneeId.Value), cancellationToken)
            : null;

        task.UpdateOverview(
            Name.Create(command.Name),
            Description.Create(command.Description),
            IsCompleted.Create(command.IsCompleted),
            assignee);
        task.Stage.Sprint.Project.RefreshActivity();

        return new UpdateOverviewProjectTaskResult();
    }
}
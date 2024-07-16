using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Move;

public sealed class MoveProjectTaskCommandHandler(
    IProjectSprintRepository projectSprintRepository,
    IProjectTaskRepository projectTaskRepository)
    : IRequestHandler<MoveProjectTaskCommand, ErrorOr<MoveProjectTaskResult>>
{
    public async Task<ErrorOr<MoveProjectTaskResult>> Handle(MoveProjectTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await projectTaskRepository.GetWithSprint(
            ProjectTaskId.Create(command.Id),
            false,
            cancellationToken: cancellationToken);
        if (task is null) return Errors.ProjectTask.NotFound;

        var stages = await projectSprintRepository.GetStagesWithTasks(
            task.Stage.Sprint.Id,
            cancellationToken);
        // replace with tracking entity
        task = stages
            .Single(s => s == task.Stage)
            .Tasks
            .Single(t => t.Id == task.Id);

        if (!task.Move(command.OverId, stages)) 
            return Errors.ProjectTask.OverNotFound;

        task.Stage.Sprint.Project.RefreshActivity();

        return new MoveProjectTaskResult();
    }
}
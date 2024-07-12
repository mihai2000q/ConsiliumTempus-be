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
        var task = await projectTaskRepository.GetWithWorkspace(
            ProjectTaskId.Create(command.Id),
            cancellationToken);
        if (task is null) return Errors.ProjectTask.NotFound;

        var stages = await projectSprintRepository.GetStages(task.Stage.Sprint.Id, cancellationToken);

        var result = task.Move(command.OverId, stages);
        if (!result) return Errors.ProjectTask.OverNotFound;

        task.Stage.Sprint.Project.RefreshActivity();

        return new MoveProjectTaskResult();
    }
}
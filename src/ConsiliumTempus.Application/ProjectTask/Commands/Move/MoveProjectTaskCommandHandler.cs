using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Move;

public sealed class MoveProjectTaskCommandHandler(IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<MoveProjectTaskCommand, ErrorOr<MoveProjectTaskResult>>
{
    public async Task<ErrorOr<MoveProjectTaskResult>> Handle(MoveProjectTaskCommand command,
        CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.GetWithTasksAndWorkspace(
            ProjectSprintId.Create(command.SprintId), 
            cancellationToken);
        if (sprint is null) return Errors.ProjectSprint.NotFound;

        var task = sprint.Stages
            .SelectMany(s => s.Tasks)
            .SingleOrDefault(t => t.Id.Value == command.Id);
        if (task is null) return Errors.ProjectTask.NotFound;

        var isError = task.Move(command.OverId, task.Stage.Sprint.Stages);
        if (isError) return Errors.ProjectTask.OverNotFound;

        task.Stage.Sprint.Project.RefreshActivity();

        return new MoveProjectTaskResult();
    }
}
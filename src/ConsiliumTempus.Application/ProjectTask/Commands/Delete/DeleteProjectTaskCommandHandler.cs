using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.Events;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Delete;

internal sealed class DeleteProjectTaskCommandHandler(IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<DeleteProjectTaskCommand, ErrorOr<DeleteProjectTaskResult>>
{
    public async Task<ErrorOr<DeleteProjectTaskResult>> Handle(DeleteProjectTaskCommand command,
        CancellationToken cancellationToken)
    {
        var stage = await projectSprintRepository.GetStageWithTasksAndWorkspace(
            ProjectStageId.Create(command.StageId),
            cancellationToken);
        if (stage is null) return Errors.ProjectTask.NotFound; // the stage is not authorized, so it is not revealed

        var task = stage.Tasks.SingleOrDefault(t => t.Id.Value == command.Id);
        if (task is null) return Errors.ProjectTask.NotFound;

        stage.RemoveTask(task);
        task.AddDomainEvent(new ProjectTaskDeleted(task));
        stage.Sprint.Project.RefreshActivity();

        return new DeleteProjectTaskResult();
    }
}
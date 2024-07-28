using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;

public sealed class UpdateIsCompletedProjectTaskCommandHandler(IProjectTaskRepository projectTaskRepository)
    : IRequestHandler<UpdateIsCompletedProjectTaskCommand, ErrorOr<UpdateIsCompletedProjectTaskResult>>
{
    public async Task<ErrorOr<UpdateIsCompletedProjectTaskResult>> Handle(UpdateIsCompletedProjectTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await projectTaskRepository.GetWithWorkspace(
            ProjectTaskId.Create(command.Id),
            cancellationToken);
        if (task is null) return Errors.ProjectTask.NotFound;

        task.UpdateIsCompleted(IsCompleted.Create(
            command.IsCompleted,
            command.IsCompleted ? DateTime.UtcNow : null));
        task.Stage.Sprint.Project.RefreshActivity();

        return new UpdateIsCompletedProjectTaskResult();
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Delete;

public sealed class DeleteProjectTaskCommandHandler(IProjectTaskRepository projectTaskRepository)
    : IRequestHandler<DeleteProjectTaskCommand, ErrorOr<DeleteProjectTaskResult>>
{
    public async Task<ErrorOr<DeleteProjectTaskResult>> Handle(DeleteProjectTaskCommand command, 
        CancellationToken cancellationToken)
    {
        var task = await projectTaskRepository.Get(
            ProjectTaskId.Create(command.Id),
            cancellationToken);
        if (task is null) return Errors.ProjectTask.NotFound;
        
        task.Stage.RemoveTask(task);
        task.Stage.Sprint.Project.RefreshActivity();

        return new DeleteProjectTaskResult();
    }
}
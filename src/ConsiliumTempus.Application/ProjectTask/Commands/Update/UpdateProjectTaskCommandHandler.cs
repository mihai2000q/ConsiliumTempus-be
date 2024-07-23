using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Update;

public sealed class UpdateProjectTaskCommandHandler(
    IProjectTaskRepository projectTaskRepository,
    IUserRepository userRepository)
    : IRequestHandler<UpdateProjectTaskCommand, ErrorOr<UpdateProjectTaskResult>>
{
    public async Task<ErrorOr<UpdateProjectTaskResult>> Handle(UpdateProjectTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await projectTaskRepository.GetWithWorkspace(
            ProjectTaskId.Create(command.Id),
            cancellationToken);
        if (task is null) return Errors.ProjectTask.NotFound;

        var assignee = command.AssigneeId is not null
            ? await userRepository.Get(UserId.Create(command.AssigneeId.Value), cancellationToken)
            : null;

        task.Update(
            Name.Create(command.Name),
            assignee);
        task.Stage.Sprint.Project.RefreshActivity();

        return new UpdateProjectTaskResult();
    }
}
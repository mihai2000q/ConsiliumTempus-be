using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Create;

public sealed class CreateProjectTaskCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectStageRepository projectStageRepository)
    : IRequestHandler<CreateProjectTaskCommand, ErrorOr<CreateProjectTaskResult>>
{
    public async Task<ErrorOr<CreateProjectTaskResult>> Handle(CreateProjectTaskCommand command, 
        CancellationToken cancellationToken)
    {
        var stage = await projectStageRepository.GetWithTasksAndWorkspace(
            ProjectStageId.Create(command.ProjectStageId),
            cancellationToken);
        if (stage is null) return Errors.ProjectStage.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        var task = ProjectTaskAggregate.Create(
            Name.Create(command.Name),
            Description.Create(string.Empty),
            CustomOrderPosition.Create(stage.Tasks.Count),
            user,
            stage);
        stage.AddTask(task, command.OnTop);
        stage.Sprint.Project.RefreshActivity();

        return new CreateProjectTaskResult();
    }
}
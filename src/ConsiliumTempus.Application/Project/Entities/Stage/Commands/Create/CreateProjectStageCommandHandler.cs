using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;

public sealed class CreateProjectStageCommandHandler(IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<CreateProjectStageCommand, ErrorOr<CreateProjectStageResult>>
{
    public async Task<ErrorOr<CreateProjectStageResult>> Handle(CreateProjectStageCommand command,
        CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.GetWithStagesAndWorkspace(
            ProjectSprintId.Create(command.ProjectSprintId),
            cancellationToken);
        if (sprint is null) return Errors.ProjectSprint.NotFound;

        var stage = ProjectStage.Create(
            Name.Create(command.Name),
            CustomOrderPosition.Create(command.OnTop ? 0 : sprint.Stages.Count),
            sprint);
        sprint.AddStage(stage, command.OnTop);
        sprint.Project.RefreshActivity();

        return new CreateProjectStageResult();
    }
}
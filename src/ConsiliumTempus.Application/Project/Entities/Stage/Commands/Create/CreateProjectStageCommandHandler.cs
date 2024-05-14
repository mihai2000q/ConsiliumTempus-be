using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;

public sealed class CreateProjectStageCommandHandler(
    IProjectSprintRepository projectSprintRepository,
    IProjectStageRepository projectStageRepository)
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
            CustomOrderPosition.Create(sprint.Stages.Count),
            sprint);
        await projectStageRepository.Add(stage, cancellationToken);

        sprint.Project.RefreshActivity();

        return new CreateProjectStageResult();
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;

public sealed class AddStageToProjectSprintCommandHandler(IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<AddStageToProjectSprintCommand, ErrorOr<AddStageToProjectSprintResult>>
{
    public async Task<ErrorOr<AddStageToProjectSprintResult>> Handle(AddStageToProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.GetWithWorkspace(
            ProjectSprintId.Create(command.Id),
            cancellationToken);
        if (sprint is null) return Errors.ProjectSprint.NotFound;

        var stage = ProjectStage.Create(
            Name.Create(command.Name),
            CustomOrderPosition.Create(command.OnTop ? 0 : sprint.Stages.Count),
            sprint);
        sprint.AddStage(stage, command.OnTop);
        sprint.Project.RefreshActivity();

        return new AddStageToProjectSprintResult();
    }
}
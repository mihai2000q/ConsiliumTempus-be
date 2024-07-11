using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;

public sealed class MoveStageFromProjectSprintCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<MoveStageFromProjectSprintCommand, ErrorOr<MoveStageFromProjectSprintResult>>
{
    public async Task<ErrorOr<MoveStageFromProjectSprintResult>> Handle(MoveStageFromProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.GetWithWorkspace(
            ProjectSprintId.Create(command.Id),
            cancellationToken);
        if (sprint is null) return Errors.ProjectSprint.NotFound;

        var stageId = ProjectStageId.Create(command.StageId);
        var stage = sprint.Stages.SingleOrDefault(s => s.Id == stageId);
        if (stage is null) return Errors.ProjectStage.NotFoundWithId(stageId);

        var overStageId = ProjectStageId.Create(command.OverStageId);
        var overStage = sprint.Stages.SingleOrDefault(s => s.Id == overStageId);
        if (overStage is null) return Errors.ProjectStage.NotFoundWithId(overStageId);

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        stage.Move(overStage, user);
        sprint.Project.RefreshActivity();

        return new MoveStageFromProjectSprintResult();
    }
}
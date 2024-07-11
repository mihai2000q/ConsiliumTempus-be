using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;

public sealed class UpdateStageFromProjectSprintCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<UpdateStageFromProjectSprintCommand, ErrorOr<UpdateStageFromProjectSprintResult>>
{
    public async Task<ErrorOr<UpdateStageFromProjectSprintResult>> Handle(UpdateStageFromProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.GetWithWorkspace(
            ProjectSprintId.Create(command.Id),
            cancellationToken);
        if (sprint is null) return Errors.ProjectSprint.NotFound;

        var stage = sprint.Stages.SingleOrDefault(s => s.Id.Value == command.StageId);
        if (stage is null) return Errors.ProjectStage.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        stage.Update(Name.Create(command.Name), user);
        sprint.Project.RefreshActivity();

        return new UpdateStageFromProjectSprintResult();
    }
}
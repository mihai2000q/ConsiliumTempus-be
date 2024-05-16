using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;

public sealed class UpdateStageFromProjectSprintCommandHandler(IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<UpdateStageFromProjectSprintCommand, ErrorOr<UpdateStageFromProjectSprintResult>>
{
    public async Task<ErrorOr<UpdateStageFromProjectSprintResult>> Handle(UpdateStageFromProjectSprintCommand command, 
        CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.GetWithWorkspace(
            ProjectSprintId.Create(command.Id),
            cancellationToken);
        if (sprint is null) return Errors.ProjectStage.NotFound;

        var stage = sprint.Stages.SingleOrDefault(s => s.Id.Value == command.StageId);
        if (stage is null) return Errors.ProjectStage.NotFound;
        
        stage.Update(
            Name.Create(command.Name),
            stage.CustomOrderPosition);
        
        sprint.Project.RefreshActivity();

        return new UpdateStageFromProjectSprintResult();
    }
}
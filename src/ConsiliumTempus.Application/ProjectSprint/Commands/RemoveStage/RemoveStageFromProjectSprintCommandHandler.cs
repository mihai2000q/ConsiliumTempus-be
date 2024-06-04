using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;

public sealed class RemoveStageFromProjectSprintCommandHandler(
    IAuditRepository auditRepository,
    IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<RemoveStageFromProjectSprintCommand, ErrorOr<RemoveStageFromProjectSprintResult>>
{
    public async Task<ErrorOr<RemoveStageFromProjectSprintResult>> Handle(RemoveStageFromProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.GetWithWorkspace(
            ProjectSprintId.Create(command.Id),
            cancellationToken);
        if (sprint is null) return Errors.ProjectSprint.NotFound;
        if (sprint.Stages.Count == 1) return Errors.ProjectStage.OnlyOneStage;

        var stage = sprint.Stages.SingleOrDefault(s => s.Id.Value == command.StageId);
        if (stage is null) return Errors.ProjectStage.NotFound;

        sprint.RemoveStage(stage);
        sprint.Project.RefreshActivity();
        
        // Cascade deactivated for sprint as the stage has audit too (avoid cycles)
        auditRepository.Remove(sprint.Audit); 

        return new RemoveStageFromProjectSprintResult();
    }
}
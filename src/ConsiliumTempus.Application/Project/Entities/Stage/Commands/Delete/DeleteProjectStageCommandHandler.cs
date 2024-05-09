using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;

public sealed class DeleteProjectStageCommandHandler(IProjectStageRepository projectStageRepository)
    : IRequestHandler<DeleteProjectStageCommand, ErrorOr<DeleteProjectStageResult>>
{
    public async Task<ErrorOr<DeleteProjectStageResult>> Handle(DeleteProjectStageCommand command, CancellationToken cancellationToken)
    {
        var stage = await projectStageRepository.GetWithWorkspace(ProjectStageId.Create(command.Id), cancellationToken);
        if (stage is null) return Errors.ProjectStage.NotFound;
        
        projectStageRepository.Remove(stage);
        
        stage.Sprint.Project.RefreshActivity();

        return new DeleteProjectStageResult();
    }
}
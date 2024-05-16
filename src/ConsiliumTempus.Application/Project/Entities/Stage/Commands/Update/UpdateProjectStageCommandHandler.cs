using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;

public sealed class UpdateProjectStageCommandHandler(IProjectStageRepository projectStageRepository)
    : IRequestHandler<UpdateProjectStageCommand, ErrorOr<UpdateProjectStageResult>>
{
    public async Task<ErrorOr<UpdateProjectStageResult>> Handle(UpdateProjectStageCommand command, 
        CancellationToken cancellationToken)
    {
        var stage = await projectStageRepository.GetWithWorkspace(ProjectStageId.Create(command.Id), cancellationToken);
        if (stage is null) return Errors.ProjectStage.NotFound;
        
        stage.Update(
            Name.Create(command.Name),
            stage.CustomOrderPosition);
        
        stage.Sprint.Project.RefreshActivity();

        return new UpdateProjectStageResult();
    }
}
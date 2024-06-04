using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;

public sealed class AddStageToProjectSprintCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<AddStageToProjectSprintCommand, ErrorOr<AddStageToProjectSprintResult>>
{
    public async Task<ErrorOr<AddStageToProjectSprintResult>> Handle(AddStageToProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.GetWithWorkspace(
            ProjectSprintId.Create(command.Id),
            cancellationToken);
        if (sprint is null) return Errors.ProjectSprint.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        var stage = ProjectStage.Create(
            Name.Create(command.Name),
            CustomOrderPosition.Create(command.OnTop ? 0 : sprint.Stages.Count),
            sprint,
            user);
        sprint.AddStage(stage, command.OnTop);
        sprint.Project.RefreshActivity();

        return new AddStageToProjectSprintResult();
    }
}
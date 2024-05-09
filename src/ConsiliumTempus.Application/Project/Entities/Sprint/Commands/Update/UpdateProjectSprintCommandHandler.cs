using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Update;

public sealed class UpdateProjectSprintCommandHandler(IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<UpdateProjectSprintCommand, ErrorOr<UpdateProjectSprintResult>>
{
    public async Task<ErrorOr<UpdateProjectSprintResult>> Handle(UpdateProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var sprint = await projectSprintRepository.GetWithProjectAndWorkspace(
            ProjectSprintId.Create(command.Id),
            cancellationToken);
        if (sprint is null) return Errors.ProjectSprint.NotFound;
        
        sprint.Update(
            Name.Create(command.Name),
            command.StartDate,
            command.EndDate);
        
        sprint.Project.RefreshActivity();

        return new UpdateProjectSprintResult();
    }
}
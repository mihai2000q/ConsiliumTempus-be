using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;

public sealed class DeleteProjectSprintCommandHandler(
    IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<DeleteProjectSprintCommand, ErrorOr<DeleteProjectSprintResult>>
{
    public async Task<ErrorOr<DeleteProjectSprintResult>> Handle(DeleteProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var projectSprintId = ProjectSprintId.Create(command.Id);
        var projectSprint = await projectSprintRepository
            .GetWithWorkspace(projectSprintId, cancellationToken);

        if (projectSprint is null) return Errors.ProjectSprint.NotFound;

        projectSprintRepository.Remove(projectSprint);

        projectSprint.Project.RefreshActivity();
        
        return new DeleteProjectSprintResult();
    }
}
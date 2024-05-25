using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Delete;

public sealed class DeleteProjectSprintCommandHandler(
    IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<DeleteProjectSprintCommand, ErrorOr<DeleteProjectSprintResult>>
{
    public async Task<ErrorOr<DeleteProjectSprintResult>> Handle(DeleteProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var projectSprint = await projectSprintRepository.GetWithSprintsAndWorkspace(
            ProjectSprintId.Create(command.Id),
            cancellationToken);

        if (projectSprint is null) return Errors.ProjectSprint.NotFound;
        if (projectSprint.Project.Sprints.Count == 1) return Errors.ProjectSprint.OnlyOneSprint;

        projectSprintRepository.Remove(projectSprint);

        projectSprint.Project.RefreshActivity();

        return new DeleteProjectSprintResult();
    }
}
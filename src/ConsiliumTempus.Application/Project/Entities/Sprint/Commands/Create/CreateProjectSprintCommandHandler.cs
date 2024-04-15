using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;

public sealed class CreateProjectSprintCommandHandler(
    IProjectRepository projectRepository,
    IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<CreateProjectSprintCommand, ErrorOr<CreateProjectSprintResult>>
{
    public async Task<ErrorOr<CreateProjectSprintResult>> Handle(CreateProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var projectId = ProjectId.Create(command.ProjectId);
        var project = await projectRepository.GetWithWorkspace(projectId, cancellationToken);

        if (project is null) return Errors.Project.NotFound;

        var projectSprint = ProjectSprint.Create(
            Name.Create(command.Name),
            project,
            command.StartDate,
            command.EndDate);
        await projectSprintRepository.Add(projectSprint, cancellationToken);

        project.RefreshActivity();
        
        return new CreateProjectSprintResult();
    }
}
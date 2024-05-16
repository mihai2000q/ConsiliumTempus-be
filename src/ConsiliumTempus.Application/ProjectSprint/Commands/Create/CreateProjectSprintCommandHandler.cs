using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Create;

public sealed class CreateProjectSprintCommandHandler(
    IProjectRepository projectRepository,
    IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<CreateProjectSprintCommand, ErrorOr<CreateProjectSprintResult>>
{
    public async Task<ErrorOr<CreateProjectSprintResult>> Handle(CreateProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithWorkspace(ProjectId.Create(command.ProjectId), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var projectSprint = ProjectSprintAggregate.Create(
            Name.Create(command.Name),
            project,
            command.StartDate,
            command.EndDate);
        await projectSprintRepository.Add(projectSprint, cancellationToken);

        project.RefreshActivity();
        
        return new CreateProjectSprintResult();
    }
}
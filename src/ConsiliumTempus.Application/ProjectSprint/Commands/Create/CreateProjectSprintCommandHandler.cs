using ConsiliumTempus.Application.Common.Extensions;
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
        var project = await projectRepository.GetWithStagesAndWorkspace(
            ProjectId.Create(command.ProjectId),
            cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var projectSprint = ProjectSprintAggregate.Create(
            Name.Create(command.Name),
            project,
            command.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            command.EndDate);

        project.Sprints.IfNotEmpty(sprints =>
        {
            var previousSprint = sprints[0];
            if (previousSprint.EndDate is null)
                previousSprint.Update(previousSprint.Name, previousSprint.StartDate, DateOnly.FromDateTime(DateTime.UtcNow));
        });

        if (command.KeepPreviousStages)
            project.Sprints
                .IfNotEmpty(sprints =>
                    projectSprint.AddStages(sprints[0].Stages));

        await projectSprintRepository.Add(projectSprint, cancellationToken);

        project.RefreshActivity();

        return new CreateProjectSprintResult();
    }
}
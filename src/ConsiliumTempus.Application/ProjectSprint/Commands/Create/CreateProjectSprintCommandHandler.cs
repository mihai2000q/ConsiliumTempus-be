using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Create;

internal sealed class CreateProjectSprintCommandHandler(
    IProjectRepository projectRepository,
    IProjectSprintRepository projectSprintRepository)
    : IRequestHandler<CreateProjectSprintCommand, ErrorOr<CreateProjectSprintResult>>
{
    public async Task<ErrorOr<CreateProjectSprintResult>> Handle(CreateProjectSprintCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetWithStages(
            ProjectId.Create(command.ProjectId),
            cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var projectSprint = ProjectSprintAggregate.Create(
            Name.Create(command.Name),
            project,
            command.StartDate,
            command.EndDate);

        project.Sprints.IfNotEmpty(sprints =>
        {
            var previousSprint = sprints[0];
            if (previousSprint.EndDate is null)
                previousSprint.UpdateEndDate(DateOnly.FromDateTime(DateTime.UtcNow));
        });

        if (command.KeepPreviousStages)
        {
            project.Sprints.IfNotEmpty(sprints =>
                projectSprint.AddStages(sprints[0].Stages
                    .Select(s => s.CopyToSprint(projectSprint))
                    .ToList())
            );
        }
        else
        {
            projectSprint.AddStage(ProjectStage.Create(
                Name.Create(Constants.ProjectStage.Names[0]),
                CustomOrderPosition.Create(0),
                projectSprint));
        }

        await projectSprintRepository.Add(projectSprint, cancellationToken);

        if (command.ProjectStatus is not null)
        {
            project.AddStatus(ProjectStatus.Create(
                Title.Create(command.ProjectStatus.Title),
                command.ProjectStatus.Status,
                Description.Create(command.ProjectStatus.Description),
                project));
        }

        project.RefreshActivity();

        return new CreateProjectSprintResult();
    }
}
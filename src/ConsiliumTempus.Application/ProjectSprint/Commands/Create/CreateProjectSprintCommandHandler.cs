using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Create;

public sealed class CreateProjectSprintCommandHandler(
    ICurrentUserProvider currentUserProvider,
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

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        var projectSprint = ProjectSprintAggregate.Create(
            Name.Create(command.Name),
            project,
            user,
            command.StartDate,
            command.EndDate);

        project.Sprints.IfNotEmpty(sprints =>
        {
            var previousSprint = sprints[0];
            if (previousSprint.EndDate is null)
                previousSprint.Update(
                    previousSprint.Name,
                    previousSprint.StartDate,
                    DateOnly.FromDateTime(DateTime.UtcNow),
                    user);
        });

        if (command.KeepPreviousStages)
        {
            project.Sprints.IfNotEmpty(sprints =>
                    projectSprint.AddStages(sprints[0].Stages));
        }
        else
        {
            projectSprint.AddStage(ProjectStage.Create(
                Name.Create(Constants.ProjectStage.Names[0]),
                CustomOrderPosition.Create(0),
                projectSprint,
                user));
        }

        await projectSprintRepository.Add(projectSprint, cancellationToken);

        if (command.ProjectStatus is not null)
        {
            project.AddStatus(ProjectStatus.Create(
                Title.Create(command.ProjectStatus.Title), 
                Enum.Parse<ProjectStatusType>(command.ProjectStatus.Status),
                Description.Create(command.ProjectStatus.Description), 
                project,
                user));
        }

        project.RefreshActivity();

        return new CreateProjectSprintResult();
    }
}
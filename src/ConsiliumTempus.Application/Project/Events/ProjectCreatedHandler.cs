using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using MediatR;

namespace ConsiliumTempus.Application.Project.Events;

public sealed class ProjectCreatedHandler : INotificationHandler<ProjectCreated>
{
    public Task Handle(ProjectCreated notification, CancellationToken cancellationToken)
    {
        var sprint = ProjectSprintAggregate.Create(
            Name.Create(Constants.ProjectSprint.Name),
            notification.Project,
            notification.Project.Owner,
            DateOnly.FromDateTime(DateTime.UtcNow));
        notification.Project.AddSprint(sprint);

        var count = 0;
        Constants.ProjectStage.Names
            .Select(name => ProjectStage.Create(
                Name.Create(name),
                CustomOrderPosition.Create(count++), 
                sprint,
                notification.Project.Owner))
            .ToList()
            .ForEach(stage => sprint.AddStage(stage));

        count = 0;
        var stage = sprint.Stages[0];
        Constants.ProjectTask.Names
            .ToList()
            .ForEach(name => stage.AddTask(ProjectTaskAggregate.Create(
                Name.Create(name),
                Description.Create(string.Empty),
                CustomOrderPosition.Create(count++),
                notification.Project.Owner,
                stage)));

        return Task.CompletedTask;
    }
}
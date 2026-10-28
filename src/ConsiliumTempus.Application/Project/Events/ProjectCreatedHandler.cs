using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using MediatR;

namespace ConsiliumTempus.Application.Project.Events;

internal sealed class ProjectCreatedHandler : INotificationHandler<ProjectCreated>
{
    public Task Handle(ProjectCreated notification, CancellationToken cancellationToken)
    {
        var sprint = ProjectSprintAggregate.Create(
            Name.Create(Constants.ProjectSprint.Name),
            notification.Project,
            DateOnly.FromDateTime(DateTime.UtcNow));
        notification.Project.AddSprint(sprint);

        Constants.ProjectStage.Names
            .Select((name, index) => ProjectStage.Create(
                Name.Create(name),
                CustomOrderPosition.Create(index),
                sprint))
            .ForEach(stage => sprint.AddStage(stage));

        var stage = sprint.Stages[0];
        Constants.ProjectTask.Names
            .Select((name, index) => ProjectTaskAggregate.Create(
                Name.Create(name),
                Description.Create(string.Empty),
                CustomOrderPosition.Create(index),
                stage))
            .ForEach(task => stage.AddTask(task));

        return Task.CompletedTask;
    }
}
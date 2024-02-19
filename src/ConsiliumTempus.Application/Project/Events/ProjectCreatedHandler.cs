using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.ProjectTask;
using MediatR;

namespace ConsiliumTempus.Application.Project.Events;

public sealed class ProjectCreatedHandler : INotificationHandler<ProjectCreated>
{
    public Task Handle(ProjectCreated notification, CancellationToken cancellationToken)
    {
        var sprint = ProjectSprint.Create(Constants.ProjectSprint.Name, notification.Project);
        notification.Project.AddSprint(sprint);

        var count = 0;
        Constants.ProjectSection.Names
            .Select(name => ProjectSection.Create(name, count++, sprint))
            .ToList()
            .ForEach(section => sprint.AddSection(section));

        count = 0;
        var section = sprint.Sections[0];
        Constants.ProjectTask.Names
            .ToList()
            .ForEach(name => section.AddTask(ProjectTaskAggregate.Create(
                name, 
                string.Empty, 
                count++,
                notification.User,
                section)));

        return Task.CompletedTask;
    }
}
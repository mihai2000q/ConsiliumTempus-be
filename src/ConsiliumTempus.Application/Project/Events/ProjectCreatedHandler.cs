using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Events;
using MediatR;

namespace ConsiliumTempus.Application.Project.Events;

public sealed class ProjectCreatedHandler : INotificationHandler<ProjectCreated>
{
    public Task Handle(ProjectCreated notification, CancellationToken cancellationToken)
    {
        var sprint = ProjectSprint.Create(Constants.ProjectSprint.Name, notification.Project);
        notification.Project.AddSprint(sprint);

        Constants.ProjectSection.Names
            .Select(name => ProjectSection.Create(name, sprint))
            .ToList()
            .ForEach(section => sprint.AddSection(section));

        Constants.ProjectTask.Names
            .ToList()
            .ForEach(name => ProjectTask.Create(
                name, 
                string.Empty, 
                notification.User,
                sprint.Sections[0]));

        return Task.CompletedTask;
    }
}
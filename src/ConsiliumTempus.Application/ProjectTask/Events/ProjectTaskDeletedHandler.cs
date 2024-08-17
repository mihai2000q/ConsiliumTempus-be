using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.ProjectTask.Events;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Events;

public sealed class ProjectTaskDeletedHandler(IProjectTaskRepository projectTaskRepository) 
    : INotificationHandler<ProjectTaskDeleted>
{
    public async Task Handle(ProjectTaskDeleted notification, CancellationToken cancellationToken)
    {
        var task = notification.ProjectTask;

        await projectTaskRepository.DeleteCustomFieldsByTask(task.Id, cancellationToken);
    }
}
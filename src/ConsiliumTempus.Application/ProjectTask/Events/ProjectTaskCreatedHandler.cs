using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.Events;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Events;

public sealed class ProjectTaskCreatedHandler(ICustomFieldSetupRepository customFieldSetupRepository) 
    : INotificationHandler<ProjectTaskCreated>
{
    public async Task Handle(ProjectTaskCreated notification, CancellationToken cancellationToken)
    {
        var task = notification.ProjectTask;
        var customFieldSetups = await customFieldSetupRepository.GetList(
            null,
            task.Stage.Sprint.Project.Id,
            cancellationToken);
        customFieldSetups.ForEach(setup => task.AddCustomField(CustomField.Create(setup, task)));
    }
}
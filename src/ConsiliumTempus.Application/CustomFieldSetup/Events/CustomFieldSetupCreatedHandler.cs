using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Events;

public sealed class CustomFieldSetupCreatedHandler(IProjectTaskRepository projectTaskRepository)
    : INotificationHandler<CustomFieldSetupCreated>
{
    public async Task Handle(CustomFieldSetupCreated notification, CancellationToken cancellationToken)
    {
        var setup = notification.CustomFieldSetup;
        if (setup.Project is null) return;

        var tasks = await projectTaskRepository.GetListByProject(setup.Project.Id, cancellationToken);
        tasks.ForEach(task => task.AddCustomField(CustomField.Create(setup)));
    }
}
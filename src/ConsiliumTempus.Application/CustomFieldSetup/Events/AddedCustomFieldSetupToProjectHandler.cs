using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Events;

public sealed class AddedCustomFieldSetupToProjectHandler(IProjectTaskRepository projectTaskRepository)
    : INotificationHandler<AddedCustomFieldSetupToProject>
{
    public async Task Handle(AddedCustomFieldSetupToProject notification, CancellationToken cancellationToken)
    {
        var (setup, project) = notification;
        var tasks = await projectTaskRepository.GetListByProject(project.Id, cancellationToken);
        tasks.ForEach(task => task.AddCustomField(CustomField.Create(setup, task)));
    }
}
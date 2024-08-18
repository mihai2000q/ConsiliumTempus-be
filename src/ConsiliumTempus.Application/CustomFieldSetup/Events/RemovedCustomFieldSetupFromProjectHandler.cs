using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Events;

public sealed class RemovedCustomFieldSetupFromProjectHandler(IProjectTaskRepository projectTaskRepository)
    : INotificationHandler<RemovedCustomFieldSetupFromProject>
{
    public async Task Handle(RemovedCustomFieldSetupFromProject notification, CancellationToken cancellationToken)
    {
        var (setup, project) = notification;
        await projectTaskRepository.DeleteCustomFieldsByProjectAndSetup(setup, project, cancellationToken);
    }
}
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Events;

public sealed class RemovedOptionFromMultiSelectCustomFieldSetupHandler(IProjectTaskRepository projectTaskRepository) 
    : INotificationHandler<RemovedOptionFromMultiSelectCustomFieldSetup>
{
    public async Task Handle(RemovedOptionFromMultiSelectCustomFieldSetup notification, CancellationToken cancellationToken)
    {
        var (setup, option) = notification;

        var customFields = await projectTaskRepository.GetMultiSelectCustomFieldsBySetup(
            setup, 
            cancellationToken);
        customFields.ForEach(cf => cf.RemoveOption(option));
    }
}
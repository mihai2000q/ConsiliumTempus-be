using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Events;

internal sealed class RemovedOptionFromSingleSelectCustomFieldSetupHandler(IProjectTaskRepository projectTaskRepository)
    : INotificationHandler<RemovedOptionFromSingleSelectCustomFieldSetup>
{
    public async Task Handle(RemovedOptionFromSingleSelectCustomFieldSetup notification, CancellationToken cancellationToken)
    {
        var (setup, option) = notification;

        var customFields = await projectTaskRepository.GetSingleSelectCustomFieldsBySetup(
            setup,
            cancellationToken);
        customFields.ForEach(cf => cf.RemoveOption(option));
    }
}

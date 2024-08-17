using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.Events;
using MediatR;

namespace ConsiliumTempus.Application.Project.Events;

public sealed class ProjectDeletedHandler(ICustomFieldSetupRepository customFieldSetupRepository)
    : INotificationHandler<ProjectDeleted>
{
    public async Task Handle(ProjectDeleted notification, CancellationToken cancellationToken)
    {
        var project = notification.Project;

        await customFieldSetupRepository.DeleteByProject(
            project.Id,
            cancellationToken);
    }
}
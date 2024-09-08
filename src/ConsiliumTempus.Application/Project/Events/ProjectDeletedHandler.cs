using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Project.Events;
using MediatR;

namespace ConsiliumTempus.Application.Project.Events;

public sealed class ProjectDeletedHandler(
    ICustomFieldSetupRepository customFieldSetupRepository,
    IProjectTaskRepository projectTaskRepository)
    : INotificationHandler<ProjectDeleted>
{
    public async Task Handle(ProjectDeleted notification, CancellationToken cancellationToken)
    {
        await projectTaskRepository.DeleteCustomFieldsByProject(notification.Project, cancellationToken);
        await customFieldSetupRepository.DeleteByProject(notification.Project, cancellationToken);
    }
}
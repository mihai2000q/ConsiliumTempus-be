using ConsiliumTempus.Domain.Project.Events;
using MediatR;

namespace ConsiliumTempus.Application.Project.Events;

public sealed class RemovedAllowedMemberFromProjectHandler : INotificationHandler<RemovedAllowedMemberFromProject>
{
    public Task Handle(RemovedAllowedMemberFromProject notification, CancellationToken cancellationToken)
    {
        notification.Project.UpdateFavorites(false, notification.User);

        return Task.CompletedTask;
    }
}
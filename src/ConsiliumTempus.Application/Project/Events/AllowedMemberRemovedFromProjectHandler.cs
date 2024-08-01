using ConsiliumTempus.Domain.Project.Events;
using MediatR;

namespace ConsiliumTempus.Application.Project.Events;

public sealed class AllowedMemberRemovedFromProjectHandler : INotificationHandler<AllowedMemberRemovedFromProject>
{
    public Task Handle(AllowedMemberRemovedFromProject notification, CancellationToken cancellationToken)
    {
        notification.Project.UpdateFavorites(false, notification.User);

        return Task.CompletedTask;
    }
}
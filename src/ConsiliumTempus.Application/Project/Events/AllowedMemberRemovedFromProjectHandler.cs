using ConsiliumTempus.Domain.Project.Events;
using MediatR;

namespace ConsiliumTempus.Application.Project.Events;

public sealed class AllowedMemberRemovedFromProjectHandler : INotificationHandler<AllowedMemberRemovedFromProject>
{
    public Task Handle(AllowedMemberRemovedFromProject notification, CancellationToken cancellationToken)
    {
        var (project, user) = notification;
        if (project.IsFavorite(user)) project.UpdateFavorites(false, user);
        return Task.CompletedTask;
    }
}
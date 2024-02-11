using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Events;
using MediatR;

namespace ConsiliumTempus.Application.Common.Events;

public sealed class MembershipCreatedHandler(IWorkspaceRoleRepository workspaceRoleRepository) 
    : INotificationHandler<MembershipCreated>
{
    public Task Handle(MembershipCreated notification, CancellationToken cancellationToken)
    {
        var role = notification.Membership.WorkspaceRole;
        workspaceRoleRepository.Attach(role);

        return Task.CompletedTask;
    }
}
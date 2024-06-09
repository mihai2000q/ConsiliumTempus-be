using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Common.IntegrationTests.Common.Entities;

public static class MembershipFactory
{
    public static Membership Create(
        UserAggregate user,
        WorkspaceAggregate workspace,
        WorkspaceRole workspaceRole,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        return EntityBuilder<Membership>.Empty()
            .WithProperty(nameof(Membership.Id), (user.Id, workspace.Id))
            .WithProperty(nameof(Membership.User), user)
            .WithProperty(nameof(Membership.Workspace), workspace)
            .WithField(nameof(Membership.WorkspaceRole).ToIdBackingField(), workspaceRole.Id)
            .WithProperty(nameof(Membership.CreatedDateTime), createdDateTime ?? DateTime.UtcNow)
            .WithProperty(nameof(Membership.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow)
            .Build();
    }
}
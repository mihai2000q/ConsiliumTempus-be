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
        var membership = DomainFactory.GetObjectInstance<Membership>();

        DomainFactory.SetField(ref membership, nameof(membership.WorkspaceRole).ToIdBackingField(), workspaceRole.Id);
        DomainFactory.SetProperty(ref membership, nameof(membership.Id), (user.Id, workspace.Id));
        DomainFactory.SetProperty(ref membership, nameof(membership.User), user);
        DomainFactory.SetProperty(ref membership, nameof(membership.Workspace), workspace);
        DomainFactory.SetProperty(ref membership, nameof(membership.CreatedDateTime), createdDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref membership, nameof(membership.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow);

        return membership;
    }
}
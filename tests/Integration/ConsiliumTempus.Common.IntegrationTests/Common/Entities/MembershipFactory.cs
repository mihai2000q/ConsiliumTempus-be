using ConsiliumTempus.Common.IntegrationTests.Extensions;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Common.IntegrationTests.Common.Entities;

public static class MembershipFactory
{
    public static Membership Create(
        UserAggregate user,
        WorkspaceAggregate workspace,
        Audit audit,
        WorkspaceRole workspaceRole)
    {
        return EntityBuilder<Membership>.Empty()
            .WithProperty(nameof(Membership.User), user)
            .WithProperty(nameof(Membership.Workspace), workspace)
            .WithField(nameof(Membership.WorkspaceRole).ToIdBackingField(), workspaceRole.Id)
            .WithProperty(nameof(Membership.Audit), audit)
            .Build();
    }
}
using ConsiliumTempus.Common.UnitTests.Common.ValueObjects;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Common.UnitTests.Common.Entities;

public static class MembershipFactory
{
    public static Membership Create(
        UserAggregate? user = null,
        WorkspaceAggregate? workspace = null,
        WorkspaceRole? workspaceRole = null)
    {
        var membership = Membership.Create(
            user ?? UserFactory.Create(),
            workspace ?? WorkspaceFactory.Create(),
            workspaceRole ?? WorkspaceRole.Member);
        membership.WithAudit(user);

        return membership;
    }

    public static List<Membership> CreateList(int count = 5)
    {
        return Enumerable
            .Repeat(0, count)
            .Select(_ => Create())
            .ToList();
    }
}
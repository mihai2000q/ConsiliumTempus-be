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
        return Membership.Create(
            user ?? UserFactory.Create(),
            workspace ?? WorkspaceFactory.Create(),
            workspaceRole ?? WorkspaceRole.Member);
    }

    public static List<Membership> CreateList(int count = 5)
    {
        return Enumerable
            .Repeat(0, count)
            .Select(_ => Create())
            .ToList();
    }
}
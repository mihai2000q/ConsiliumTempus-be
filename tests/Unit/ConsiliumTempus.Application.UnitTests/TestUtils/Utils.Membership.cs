using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Membership
    {
        internal static void Assert(
            Domain.Common.Entities.Membership membership,
            UserAggregate user,
            string workspaceName,
            string workspaceDescription)
        {
            membership.Id.Should().Be((user.Id, membership.Workspace.Id));
            membership.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            membership.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            membership.User.Should().Be(user);
            membership.WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            membership.Workspace.Id.Should().NotBeNull();
            membership.Workspace.Name.Value.Should().Be(workspaceName);
            membership.Workspace.Description.Value.Should().Be(workspaceDescription);
        }
    }
}
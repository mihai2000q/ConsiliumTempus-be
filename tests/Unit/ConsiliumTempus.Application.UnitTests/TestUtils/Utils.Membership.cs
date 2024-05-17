using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using FluentAssertions.Extensions;

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
            membership.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            membership.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            membership.User.Should().Be(user);
            membership.WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            membership.DomainEvents.Should().BeEmpty();

            membership.Workspace.Id.Value.Should().NotBeEmpty();
            membership.Workspace.Name.Value.Should().Be(workspaceName);
            membership.Workspace.Description.Value.Should().Be(workspaceDescription);
            membership.Workspace.Owner.Should().Be(user);
            membership.Workspace.IsPersonal.Value.Should().Be(true);
            membership.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            membership.Workspace.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            membership.Workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
        }
    }
}
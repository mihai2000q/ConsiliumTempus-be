using ConsiliumTempus.Domain.Common.Entities;

namespace ConsiliumTempus.Application.UnitTests.Mock;

internal static partial class Mock
{
    internal static class Membership
    {
        internal static Domain.Common.Entities.Membership CreateMock()
        {
            var user = User.CreateMock();
            var workspace = Workspace.CreateMock();
            return Domain.Common.Entities.Membership.Create(user, workspace, WorkspaceRole.Member);
        }
    }
}
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.User.Events;

internal static class UserDeletedHandlerData
{
    internal class GetData : TheoryData<List<WorkspaceAggregate>, UserAggregate>
    {
        public GetData()
        {
            var user = UserFactory.Create();
            
            var workspaceWithAdminMembers = WorkspaceFactory.Create(owner: user);
            workspaceWithAdminMembers.AddUserMembership(MembershipFactory.Create(workspaceRole: WorkspaceRole.Admin));
            workspaceWithAdminMembers.AddUserMembership(MembershipFactory.Create());
            
            var workspaceWithMembers = WorkspaceFactory.Create(owner: user);
            workspaceWithMembers.AddUserMembership(MembershipFactory.Create());
            
            var userWorkspaceWithMembers = WorkspaceFactory.Create(owner: user, isUserWorkspace: true);
            userWorkspaceWithMembers.AddUserMembership(MembershipFactory.Create());
            
            var workspaces = new List<WorkspaceAggregate>
            {
                WorkspaceFactory.Create(),
                WorkspaceFactory.Create(owner: user),
                WorkspaceFactory.Create(owner: user, isUserWorkspace: true),
                workspaceWithAdminMembers,
                workspaceWithMembers,
                userWorkspaceWithMembers
            };
            Add(workspaces, user);
        }
    }
}
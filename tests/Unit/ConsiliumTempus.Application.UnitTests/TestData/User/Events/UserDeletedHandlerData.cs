using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.User.Events;

internal static class UserDeletedHandlerData
{
    internal class GetData : TheoryData<UserAggregate, List<WorkspaceAggregate>, List<ProjectAggregate>>
    {
        public GetData()
        {
            // Use Case 1
            var user = UserFactory.Create();

            // Workspaces
            var workspaceWithAdmins = WorkspaceFactory.Create(owner: user);
            workspaceWithAdmins.AddUserMembership(MembershipFactory.Create(workspaceRole: WorkspaceRole.Admin));
            workspaceWithAdmins.AddUserMembership(MembershipFactory.Create());

            var workspaceWithMembers = WorkspaceFactory.Create(owner: user);
            workspaceWithMembers.AddUserMembership(MembershipFactory.Create());

            var personalWorkspaceWithMembers = WorkspaceFactory.Create(owner: user, isPersonal: true);
            personalWorkspaceWithMembers.AddUserMembership(MembershipFactory.Create());

            var workspaces = new List<WorkspaceAggregate>
            {
                WorkspaceFactory.Create(owner: user),
                WorkspaceFactory.Create(owner: user, isPersonal: true),
                workspaceWithAdmins,
                workspaceWithMembers,
                personalWorkspaceWithMembers
            };

            // Projects
            var privateProjectWithAllowedMembers = ProjectFactory.CreateWithAllowedMembers(
                owner: user,
                isPrivate: true);
            var projectWithAllowedMembers = ProjectFactory.CreateWithAllowedMembers(owner: user);

            var projects = new List<ProjectAggregate>
            {
                ProjectFactory.Create(owner: user),
                ProjectFactory.Create(owner: user, isPrivate: true),
                privateProjectWithAllowedMembers,
                projectWithAllowedMembers,
                ProjectFactory.Create(owner: user, workspace: workspaceWithAdmins),
                ProjectFactory.Create(owner: user, workspace: workspaceWithMembers),
            };

            Add(user, workspaces, projects);
        }
    }
}
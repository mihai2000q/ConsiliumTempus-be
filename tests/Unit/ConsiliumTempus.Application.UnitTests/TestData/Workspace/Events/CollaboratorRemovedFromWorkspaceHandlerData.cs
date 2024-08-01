using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace.Events;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Events;

internal static class CollaboratorRemovedFromWorkspaceHandlerData
{
    internal class GetData : TheoryData<CollaboratorRemovedFromWorkspace, List<ProjectAggregate>>
    {
        public GetData()
        {
            // Use Case 2
            var workspace = WorkspaceFactory.Create();
            workspace.AddUserMembership(MembershipFactory.Create(workspaceRole: WorkspaceRole.View));
            var membership = workspace.Memberships[1];
            var collaborator = membership.User;
            workspace.RemoveUserMembership(membership);
            workspace.ClearDomainEvents();

            var domainEvent = new CollaboratorRemovedFromWorkspace(workspace, collaborator);

            var projects = ProjectFactory.CreateList(count: 10);
            projects[1].UpdatePrivacy(IsPrivate.Create(true));
            projects[1].RemoveAllowedMember(projects[1].Owner);
            projects[1].ClearDomainEvents();
            projects[1].UpdateOwner(collaborator);
            projects[1].AddAllowedMember(collaborator);

            projects[2].UpdatePrivacy(IsPrivate.Create(true));
            projects[2].AddAllowedMember(collaborator);

            projects[3].AddAllowedMember(collaborator);

            projects[4].UpdateFavorites(true, collaborator);

            projects[5].AddAllowedMember(collaborator);
            projects[5].UpdateFavorites(true, collaborator);

            projects[6].UpdatePrivacy(IsPrivate.Create(true));
            projects[6].UpdateOwner(collaborator);
            projects[6].AddAllowedMember(collaborator);

            projects[7].UpdateOwner(collaborator);
            projects[7].AddAllowedMember(collaborator);

            Add(domainEvent, projects);

            // Use Case 2
            workspace = WorkspaceFactory.Create();
            workspace.AddUserMembership(MembershipFactory.Create(workspaceRole: WorkspaceRole.Admin));
            workspace.AddUserMembership(MembershipFactory.Create(workspaceRole: WorkspaceRole.Admin));
            workspace.AddUserMembership(MembershipFactory.Create(workspaceRole: WorkspaceRole.View));
            workspace.AddUserMembership(MembershipFactory.Create(workspaceRole: WorkspaceRole.Member));
            workspace.AddUserMembership(MembershipFactory.Create(workspaceRole: WorkspaceRole.Member));
            workspace.AddUserMembership(MembershipFactory.Create(workspaceRole: WorkspaceRole.Admin));
            workspace.AddUserMembership(MembershipFactory.Create(workspaceRole: WorkspaceRole.View));
            membership = workspace.Memberships[1];
            collaborator = membership.User;
            workspace.RemoveUserMembership(membership);
            workspace.ClearDomainEvents();

            domainEvent = new CollaboratorRemovedFromWorkspace(workspace, collaborator);

            projects = ProjectFactory.CreateList(count: 30);
            projects[2].UpdatePrivacy(IsPrivate.Create(true));
            projects[2].RemoveAllowedMember(projects[2].Owner);
            projects[2].ClearDomainEvents();
            projects[2].UpdateOwner(collaborator);
            projects[2].AddAllowedMember(collaborator);

            projects[3].RemoveAllowedMember(projects[3].Owner);
            projects[3].ClearDomainEvents();
            projects[3].UpdateOwner(collaborator);
            projects[3].AddAllowedMember(collaborator);

            projects[5].RemoveAllowedMember(projects[5].Owner);
            projects[5].ClearDomainEvents();
            projects[5].UpdateOwner(collaborator);
            projects[5].AddAllowedMember(collaborator);
            projects[5].AddAllowedMember(UserFactory.Create());

            projects[6].RemoveAllowedMember(projects[6].Owner);
            projects[6].ClearDomainEvents();
            projects[6].UpdateOwner(collaborator);
            projects[6].AddAllowedMember(collaborator);
            projects[6].AddAllowedMember(UserFactory.Create());
            projects[6].AddAllowedMember(UserFactory.Create());

            projects[8].UpdateOwner(collaborator);
            projects[8].AddAllowedMember(collaborator);
            projects[8].AddAllowedMember(UserFactory.Create());
            projects[8].UpdateFavorites(true, UserFactory.Create());
            projects[8].UpdateFavorites(true, collaborator);

            projects[12].UpdatePrivacy(IsPrivate.Create(true));
            projects[12].AddAllowedMember(UserFactory.Create());

            projects[14].UpdateOwner(collaborator);
            projects[14].AddAllowedMember(collaborator);
            projects[14].UpdateFavorites(true, collaborator);

            projects[15].UpdateFavorites(true, collaborator);
            projects[15].UpdateFavorites(true, UserFactory.Create());

            projects[16].AddAllowedMember(collaborator);
            projects[16].UpdateFavorites(true, collaborator);

            projects[20].UpdatePrivacy(IsPrivate.Create(true));
            projects[20].AddAllowedMember(collaborator);
            projects[20].UpdateFavorites(true, collaborator);
            projects[20].UpdateFavorites(true, UserFactory.Create());

            projects[23].AddAllowedMember(collaborator);
            projects[23].UpdateFavorites(true, collaborator);
            projects[23].UpdateFavorites(true, UserFactory.Create());

            projects[24].UpdateOwner(collaborator);
            projects[24].AddAllowedMember(collaborator);
            projects[24].UpdateFavorites(true, collaborator);
            projects[24].UpdateFavorites(true, UserFactory.Create());

            projects[28].UpdatePrivacy(IsPrivate.Create(true));
            projects[28].UpdateOwner(collaborator);
            projects[28].AddAllowedMember(collaborator);
            projects[28].UpdateFavorites(true, collaborator);
            projects[28].UpdateFavorites(true, UserFactory.Create());

            Add(domainEvent, projects);
        }
    }
}
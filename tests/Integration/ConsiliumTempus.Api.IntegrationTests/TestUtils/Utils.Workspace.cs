using ConsiliumTempus.Api.Contracts.Workspace.AcceptInvitation;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.GetInvitations;
using ConsiliumTempus.Api.Contracts.Workspace.GetOverview;
using ConsiliumTempus.Api.Contracts.Workspace.InviteCollaborator;
using ConsiliumTempus.Api.Contracts.Workspace.RejectInvitation;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateFavorites;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateOverview;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class Workspace
    {
        internal static void AssertGetResponse(
            GetWorkspaceResponse response,
            WorkspaceAggregate workspace,
            UserAggregate currentUser)
        {
            response.Name.Should().Be(workspace.Name.Value);
            response.IsFavorite.Should().Be(workspace.IsFavorite(currentUser));
            response.IsPersonal.Should().Be(workspace.IsPersonal.Value);
            AssertUserResponse(response.Owner, workspace.Owner);
        }
        
        internal static void AssertGetOverviewResponse(
            GetOverviewWorkspaceResponse response,
            WorkspaceAggregate workspace)
        {
            response.Description.Should().Be(workspace.Description.Value);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionWorkspaceResponse response,
            List<WorkspaceAggregate> workspaces,
            int totalCount,
            UserAggregate user,
            bool isOrdered = false)
        {
            response.Workspaces.Should().HaveCount(workspaces.Count);
            if (isOrdered)
            {
                response.Workspaces.Zip(workspaces)
                    .Should().AllSatisfy(x => AssertWorkspaceResponse(x.First, x.Second, user));
            }
            else
            {
                response.Workspaces
                    .OrderBy(w => w.Id)
                    .Zip(workspaces.OrderBy(w => w.Id.Value))
                    .Should().AllSatisfy(x => AssertWorkspaceResponse(x.First, x.Second, user));
            }

            response.TotalCount.Should().Be(totalCount);
        }

        internal static void AssertGetCollaboratorsResponse(
            GetCollaboratorsFromWorkspaceResponse response,
            IEnumerable<UserAggregate> collaborators)
        {
            response.Collaborators
                .Zip(collaborators.OrderBy(c => c.FirstName.Value))
                .Should().AllSatisfy(x => AssertUserResponse(x.First, x.Second));
        }

        internal static void AssertGetInvitationsResponse(
            GetInvitationsWorkspaceResponse response,
            IEnumerable<WorkspaceInvitation> invitations,
            int totalCount)
        {
            response.Invitations
                .Zip(invitations.OrderByDescending(i => i.CreatedDateTime))
                .Should().AllSatisfy(x => AssertWorkspaceInvitationResponse(x.First, x.Second));
            response.TotalCount.Should().Be(totalCount);
        }

        internal static void AssertCreation(
            WorkspaceAggregate workspace,
            CreateWorkspaceRequest request,
            UserAggregate user)
        {
            workspace.Id.Value.Should().NotBeEmpty();
            workspace.Name.Value.Should().Be(request.Name);
            workspace.Description.Value.Should().BeEmpty();
            workspace.Owner.Should().Be(user);
            workspace.IsPersonal.Value.Should().Be(false);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            workspace.Memberships.Should().HaveCount(1);
            workspace.Memberships[0].User.Should().Be(user);
            workspace.Memberships[0].WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            workspace.Memberships[0].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.Memberships[0].UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertInviteCollaborator(
            InviteCollaboratorToWorkspaceRequest request,
            WorkspaceAggregate workspace,
            UserAggregate sender,
            UserAggregate collaborator)
        {
            workspace.Id.Value.Should().Be(request.Id);
            workspace.Invitations.Should().ContainSingle(i => i.Collaborator == collaborator);

            var invitation = workspace.Invitations.Single(i => i.Collaborator == collaborator);
            invitation.Id.Value.Should().NotBeEmpty();
            invitation.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            invitation.Sender.Should().Be(sender);
            invitation.Collaborator.Should().Be(collaborator);
            invitation.Workspace.Should().Be(workspace);
        }

        internal static void AssertAcceptInvitation(
            AcceptInvitationToWorkspaceRequest request,
            WorkspaceAggregate workspace,
            UserAggregate collaborator)
        {
            workspace.Id.Value.Should().Be(request.Id);
            workspace.Invitations.Should().NotContain(i => i.Collaborator == collaborator);
            workspace.Memberships.Should().ContainSingle(i => i.User == collaborator);

            var membership = workspace.Memberships.Single(i => i.User == collaborator);
            membership.Id.UserId.Should().Be(collaborator.Id);
            membership.User.Should().Be(collaborator);
            membership.Workspace.Should().Be(workspace);
            membership.WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            membership.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            membership.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertRejectInvitation(
            RejectInvitationToWorkspaceRequest request,
            WorkspaceAggregate workspace,
            UserAggregate collaborator)
        {
            workspace.Id.Value.Should().Be(request.Id);
            workspace.Invitations.Should().NotContain(i => i.Collaborator == collaborator);
            workspace.Memberships.Should().NotContain(i => i.User == collaborator);
        }

        internal static void AssertUpdated(
            WorkspaceAggregate workspace,
            WorkspaceAggregate newWorkspace,
            UpdateWorkspaceRequest request)
        {
            // unchanged
            newWorkspace.Id.Value.Should().Be(request.Id);
            newWorkspace.CreatedDateTime.Should().Be(workspace.CreatedDateTime);

            // changed
            newWorkspace.Name.Value.Should().Be(request.Name);
            newWorkspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newWorkspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdatedFavorites(
            WorkspaceAggregate newWorkspace,
            UpdateFavoritesWorkspaceRequest request,
            UserAggregate user)
        {
            // unchanged
            newWorkspace.Id.Value.Should().Be(request.Id);

            // changed
            newWorkspace.IsFavorite(user).Should().Be(request.IsFavorite);
        }
        
        internal static void AssertUpdatedOverview(
            WorkspaceAggregate newWorkspace,
            UpdateOverviewWorkspaceRequest request)
        {
            // unchanged
            newWorkspace.Id.Value.Should().Be(request.Id);

            // changed
            newWorkspace.Description.Value.Should().Be(request.Description);
            newWorkspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newWorkspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
        
        private static void AssertUserResponse(
            GetWorkspaceResponse.UserResponse response,
            UserAggregate user)
        {
            response.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.FirstName + " " + user.LastName);
            response.Email.Should().Be(user.Credentials.Email);
        }

        private static void AssertWorkspaceResponse(
            GetCollectionWorkspaceResponse.WorkspaceResponse response,
            WorkspaceAggregate workspace,
            UserAggregate currentUser)
        {
            response.Id.Should().Be(workspace.Id.ToString());
            response.Name.Should().Be(workspace.Name.Value);
            response.Description.Should().Be(workspace.Description.Value);
            response.IsFavorite.Should().Be(workspace.IsFavorite(currentUser));
            response.IsPersonal.Should().Be(workspace.IsPersonal.Value);

            var owner = workspace.Owner;
            response.Owner.Id.Should().Be(owner.Id.Value);
            response.Owner.Name.Should().Be(owner.FirstName + " " + owner.LastName);
            response.Owner.Email.Should().Be(owner.Credentials.Email);
        }
        
        private static void AssertUserResponse(
            GetCollaboratorsFromWorkspaceResponse.UserResponse response,
            UserAggregate user)
        {
            response.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.FirstName + " " + user.LastName);
            response.Email.Should().Be(user.Credentials.Email);
        }

        private static void AssertWorkspaceInvitationResponse(
            GetInvitationsWorkspaceResponse.WorkspaceInvitationResponse response,
            WorkspaceInvitation invitation)
        {
            response.Id.Should().Be(invitation.Id.Value);
            AssertUserResponse(response.Sender, invitation.Sender);
            AssertUserResponse(response.Collaborator, invitation.Collaborator);
            AssertWorkspaceResponse(response.Workspace, invitation.Workspace);
        }

        private static void AssertUserResponse(
            GetInvitationsWorkspaceResponse.UserResponse response,
            UserAggregate user)
        {
            response.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.FirstName.Value + " " + user.LastName.Value);
            response.Email.Should().Be(user.Credentials.Email);
        }

        private static void AssertWorkspaceResponse(
            GetInvitationsWorkspaceResponse.WorkspaceResponse response,
            WorkspaceAggregate workspace)
        {
            response.Id.Should().Be(workspace.Id.Value);
            response.Name.Should().Be(workspace.Name.Value);
            response.IsPersonal.Should().Be(workspace.IsPersonal.Value);
        }
    }
}
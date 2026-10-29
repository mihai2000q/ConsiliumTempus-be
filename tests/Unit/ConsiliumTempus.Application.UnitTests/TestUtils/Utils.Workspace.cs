using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.KickCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.Leave;
using ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Commands.UpdateCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Domain.Workspace.Events;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Workspace
    {
        internal static bool AssertFromCreateCommand(
            WorkspaceAggregate workspace,
            CreateWorkspaceCommand command,
            UserAggregate user)
        {
            workspace.Name.Value.Should().Be(command.Name);
            workspace.Description.Value.Should().BeEmpty();
            workspace.Owner.Should().Be(user);
            workspace.IsFavorite(user).Should().Be(false);
            workspace.IsPersonal.Value.Should().Be(false);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.DomainEvents.Should().BeEmpty();

            workspace.Memberships.Should().HaveCount(1);
            workspace.Memberships[0].Id.Should().Be((user.Id, workspace.Id));
            workspace.Memberships[0].User.Should().Be(user);
            workspace.Memberships[0].Workspace.Should().Be(workspace);
            workspace.Memberships[0].WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            workspace.Projects.Should().BeEmpty();
            workspace.Favorites.Should().BeEmpty();

            return true;
        }

        internal static void AssertFromInviteCollaboratorCommand(
            InviteCollaboratorToWorkspaceCommand command,
            WorkspaceAggregate workspace,
            UserAggregate sender,
            UserAggregate collaborator)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Invitations.Should().HaveCount(1);

            var invitation = workspace.Invitations[0];
            invitation.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            invitation.Sender.Should().Be(sender);
            invitation.Collaborator.Should().Be(collaborator);
            invitation.Workspace.Should().Be(workspace);
        }

        internal static void AssertFromAcceptInvitationCommand(
            AcceptInvitationToWorkspaceCommand command,
            WorkspaceAggregate workspace,
            UserAggregate user,
            List<WorkspaceInvitation> remainingInvitations)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.Invitations.Should().Equal(remainingInvitations);
            workspace.Memberships.Should().ContainSingle(m => m.User == user);

            var membership = workspace.Memberships.Single(m => m.User == user);
            membership.Id.UserId.Should().Be(user.Id);
            membership.Id.WorkspaceId.Should().Be(workspace.Id);
            membership.User.Should().Be(user);
            membership.Workspace.Should().Be(workspace);
            membership.WorkspaceRole.Should().Be(WorkspaceRole.View);
        }

        internal static void AssertFromKickCollaboratorCommand(
            KickCollaboratorFromWorkspaceCommand command,
            WorkspaceAggregate workspace)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Memberships.Should().NotContain(m => m.User.Id.Value == command.CollaboratorId);
        }

        internal static void AssertFromRejectInvitationCommand(
            RejectInvitationToWorkspaceCommand command,
            WorkspaceAggregate workspace,
            List<WorkspaceInvitation> remainingInvitations)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Invitations.Should().Equal(remainingInvitations);
        }

        internal static void AssertFromLeaveCommand(
            LeaveWorkspaceCommand command,
            WorkspaceAggregate workspace,
            UserAggregate user)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Memberships.Should().NotContain(m => m.User == user);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateCommand(
            WorkspaceAggregate workspace,
            UpdateWorkspaceCommand command)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Name.Value.Should().Be(command.Name);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateCollaboratorCommand(
            WorkspaceAggregate workspace,
            UpdateCollaboratorFromWorkspaceCommand command)
        {
            workspace.Id.Value.Should().Be(command.Id);

            workspace.Memberships.Should().Contain(m => m.User.Id.Value == command.CollaboratorId);
            var collaboratorMembership = workspace.Memberships
                .Single(m => m.User.Id.Value == command.CollaboratorId);
            collaboratorMembership.WorkspaceRole.Name.Should().Be(command.WorkspaceRole.Capitalize());

            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateFavoritesCommand(
            WorkspaceAggregate workspace,
            UpdateFavoritesWorkspaceCommand command,
            UserAggregate currentUser)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.IsFavorite(currentUser).Should().Be(command.IsFavorite);
        }

        internal static void AssertFromUpdateOverviewCommand(
            WorkspaceAggregate workspace,
            UpdateOverviewWorkspaceCommand command)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Description.Value.Should().Be(command.Description);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateOwnerCommand(
            WorkspaceAggregate workspace,
            UpdateOwnerWorkspaceCommand command)
        {
            workspace.Id.Value.Should().Be(command.Id);
            workspace.Owner.Id.Value.Should().Be(command.OwnerId);
            workspace.Memberships.Single(m => m.User == workspace.Owner).WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromRemovedCollaborator(
            CollaboratorRemovedFromWorkspace domainEvent,
            List<ProjectAggregate> projects,
            Dictionary<ProjectAggregate, UserAggregate> previousOwners,
            List<ProjectAggregate> allowedMemberProjects,
            List<ProjectAggregate> favoriteOnlyProjects)
        {
            var (workspace, collaborator) = domainEvent;
            workspace.IsFavorite(collaborator).Should().BeFalse();

            projects.Should().AllSatisfyOrEmpty(p =>
            {
                p.Owner.Should().NotBe(collaborator);
                p.AllowedMembers.Should().NotContain(collaborator);
                if (previousOwners[p] != collaborator) p.Owner.Should().Be(previousOwners[p]);
            });

            var nonOwnerMemberships = workspace.Memberships
                .Where(m => m.User != workspace.Owner)
                .ToList();
            projects.Where(p => previousOwners[p] == collaborator).Should().AllSatisfyOrEmpty(p =>
            {
                p.AllowedMembers.Should().Contain(p.Owner);

                if (nonOwnerMemberships.Count == 0)
                {
                    p.Owner.Should().Be(workspace.Owner);
                    return;
                }

                var newOwnerMembership = nonOwnerMemberships.Single(m => m.User == p.Owner);
                newOwnerMembership.WorkspaceRole.Id.Should().Be(nonOwnerMemberships.Max(m => m.WorkspaceRole.Id));
            });

            projects.Where(allowedMemberProjects.Contains).Should().AllSatisfyOrEmpty(p =>
                p.DomainEvents.Should().ContainSingle(e =>
                    e is AllowedMemberRemovedFromProject && ((AllowedMemberRemovedFromProject)e).User == collaborator));
            projects.Where(p => !allowedMemberProjects.Contains(p)).Should().AllSatisfyOrEmpty(p =>
                p.DomainEvents.Should().BeEmpty());
            projects.Where(favoriteOnlyProjects.Contains).Should().AllSatisfyOrEmpty(p =>
                p.IsFavorite(collaborator).Should().BeFalse());
        }

        internal static void AssertWorkspace(
            GetWorkspaceResult outcome,
            WorkspaceAggregate expected,
            UserAggregate user)
        {
            outcome.Workspace.Id.Should().Be(expected.Id);
            outcome.Workspace.Name.Should().Be(expected.Name);
            outcome.Workspace.Description.Should().Be(expected.Description);
            outcome.Workspace.IsFavorite(user).Should().Be(expected.IsFavorite(user));
            outcome.Workspace.LastActivity.Should().Be(expected.LastActivity);
            outcome.Workspace.Audit.CreatedDateTime.Should().Be(expected.Audit.CreatedDateTime);
            outcome.Workspace.Audit.UpdatedDateTime.Should().Be(expected.Audit.UpdatedDateTime);
            outcome.Workspace.Memberships.Should().BeEquivalentTo(expected.Memberships);
            outcome.Workspace.Projects.Should().BeEquivalentTo(expected.Projects);
            outcome.Workspace.Favorites.Should().BeEquivalentTo(expected.Favorites);

            outcome.CurrentUser.Should().Be(user);
        }

        internal static void AssertWorkspace(
            WorkspaceAggregate outcome,
            WorkspaceAggregate expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Name.Should().Be(expected.Name);
            outcome.Description.Should().Be(expected.Description);
            outcome.LastActivity.Should().Be(expected.LastActivity);
            outcome.Audit.CreatedDateTime.Should().Be(expected.Audit.CreatedDateTime);
            outcome.Audit.UpdatedDateTime.Should().Be(expected.Audit.UpdatedDateTime);
            outcome.Memberships.Should().BeEquivalentTo(expected.Memberships);
            outcome.Projects.Should().BeEquivalentTo(expected.Projects);
            outcome.Favorites.Should().BeEquivalentTo(expected.Favorites);
        }

        internal static void AssertGetCollectionResult(
            GetCollectionWorkspaceResult result,
            GetCollectionWorkspaceQuery query,
            List<WorkspaceAggregate> workspaces,
            int workspacesCount,
            WorkspaceAggregate personalWorkspace,
            UserAggregate currentUser)
        {
            result.Workspaces.Should().BeEquivalentTo(workspaces);
            if (query.IsPersonalWorkspaceFirst) result.Workspaces.Should().HaveElementAt(0, personalWorkspace);

            result.TotalCount.Should().Be(workspacesCount);
            result.CurrentUser.Should().Be(currentUser);
        }
    }
}
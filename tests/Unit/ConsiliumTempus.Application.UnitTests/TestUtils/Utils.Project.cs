using ConsiliumTempus.Application.Project.Commands.AddAllowedMember;
using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.LeavePrivate;
using ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;
using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Commands.UpdateOwner;
using ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;
using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Project
    {
        internal static void AssertFromAddAllowedMemberCommand(
            ProjectAggregate project,
            AddAllowedMemberToProjectCommand command,
            UserAggregate collaborator)
        {
            project.Id.Value.Should().Be(command.Id);
            collaborator.Id.Value.Should().Be(command.CollaboratorId);

            project.AllowedMembers.Should().HaveCount(1);
            project.AllowedMembers[0].Should().Be(collaborator);
            
            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromAddStatusCommand(
            ProjectAggregate project,
            AddStatusToProjectCommand command,
            UserAggregate user)
        {
            project.Id.Value.Should().Be(command.Id);
            
            project.Statuses.Should().HaveCount(1);
            var status = project.LatestStatus;
            status.Should().NotBeNull();
            status!.Id.Value.Should().NotBeEmpty();
            status.Title.Value.Should().Be(command.Title);
            status.Status.ToString().ToLower().Should().Be(command.Status.ToLower());
            status.Description.Value.Should().Be(command.Description);
            status.Project.Should().Be(project);
            status.Audit.ShouldBeCreated(user);
            status.DomainEvents.Should().BeEmpty();

            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
        
        internal static bool AssertFromCreateCommand(
            ProjectAggregate project,
            CreateProjectCommand command,
            WorkspaceAggregate workspace,
            UserAggregate owner)
        {
            project.Id.Value.Should().NotBeEmpty();
            project.Name.Value.Should().Be(command.Name);
            project.Description.Value.Should().BeEmpty();
            project.IsPrivate.Value.Should().Be(command.IsPrivate);
            project.Favorites.Should().BeEmpty();
            project.IsFavorite(owner).Should().Be(false);
            project.Owner.Should().Be(owner);
            project.Lifecycle.Should().Be(ProjectLifecycle.Active);
            project.Sprints.Should().BeEmpty();
            project.Statuses.Should().BeEmpty();
            project.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.Should().Be(workspace);

            project.AllowedMembers.Should().HaveCount(1);
            project.AllowedMembers[0].Should().Be(owner);

            project.DomainEvents.Should().HaveCount(1);
            project.DomainEvents[0].Should().BeOfType<ProjectCreated>();
            ((ProjectCreated)project.DomainEvents[0]).Project.Should().Be(project);

            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            return true;
        }

        internal static bool AssertFromDeleteCommand(
            ProjectAggregate project,
            DeleteProjectCommand command)
        {
            project.Id.Value.Should().Be(command.Id);

            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            return true;
        }

        internal static void AssertFromLeavePrivateCommand(
            ProjectAggregate project,
            LeavePrivateProjectCommand command,
            UserAggregate user)
        {
            project.Id.Value.Should().Be(command.Id);
            project.AllowedMembers.Should().NotContain(u => u == user);

            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromRemoveAllowedMemberCommand(
            ProjectAggregate project,
            RemoveAllowedMemberFromProjectCommand command)
        {
            project.Id.Value.Should().Be(command.Id);
            project.AllowedMembers.Should().NotContain(s => s.Id.Value == command.AllowedMemberId);

            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromRemoveStatusCommand(
            ProjectAggregate project,
            RemoveStatusFromProjectCommand command)
        {
            project.Id.Value.Should().Be(command.Id);
            project.Statuses.Should().NotContain(s => s.Id.Value == command.StatusId);
            
            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateCommand(
            ProjectAggregate project,
            UpdateProjectCommand command)
        {
            project.Id.Value.Should().Be(command.Id);
            project.Name.Value.Should().Be(command.Name);
            project.Lifecycle.ToString().ToLower().Should().Be(command.Lifecycle.ToLower());
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateFavoritesCommand(
            ProjectAggregate project,
            UpdateFavoritesProjectCommand command,
            UserAggregate currentUser)
        {
            project.Id.Value.Should().Be(command.Id);
            project.IsFavorite(currentUser).Should().Be(command.IsFavorite);
        }

        internal static void AssertFromUpdatePrivacyCommand(
            ProjectAggregate project,
            UpdatePrivacyProjectCommand command)
        {
            project.Id.Value.Should().Be(command.Id);
            project.IsPrivate.Value.Should().Be(command.IsPrivate);
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
        
        internal static void AssertFromUpdateOverviewCommand(
            ProjectAggregate project,
            UpdateOverviewProjectCommand command)
        {
            project.Id.Value.Should().Be(command.Id);
            project.Description.Value.Should().Be(command.Description);
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUpdateOwnerCommand(
            ProjectAggregate project,
            UpdateOwnerProjectCommand command)
        {
            project.Id.Value.Should().Be(command.Id);
            project.Owner.Id.Value.Should().Be(command.OwnerId);
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
        
        internal static void AssertFromUpdateStatusCommand(
            ProjectAggregate project,
            UpdateStatusFromProjectCommand command,
            UserAggregate updatedBy)
        {
            project.Id.Value.Should().Be(command.Id);
            var status = project.Statuses.Single(s => s.Id.Value == command.StatusId);
            status.Title.Value.Should().Be(command.Title);
            status.Status.ToString().ToLower().Should().Be(command.Status.ToLower());
            status.Description.Value.Should().Be(command.Description);
            status.Audit.ShouldBeUpdated(updatedBy);

            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertProject(
            GetProjectResult outcome,
            ProjectAggregate expected,
            UserAggregate currentUser)
        {
            outcome.Project.Id.Should().Be(expected.Id);
            outcome.Project.Name.Should().Be(expected.Name);
            outcome.Project.Description.Should().Be(expected.Description);
            outcome.Project.Favorites.Should().BeEquivalentTo(expected.Favorites);
            outcome.Project.IsFavorite(currentUser).Should().Be(expected.IsFavorite(currentUser));
            outcome.Project.IsPrivate.Should().Be(expected.IsPrivate);
            outcome.Project.LastActivity.Should().Be(expected.LastActivity);
            outcome.Project.CreatedDateTime.Should().Be(expected.CreatedDateTime);
            outcome.Project.UpdatedDateTime.Should().Be(expected.UpdatedDateTime);
            outcome.Project.Workspace.Should().Be(expected.Workspace);
            outcome.Project.Sprints.Should().BeEquivalentTo(expected.Sprints);

            outcome.CurrentUser.Should().Be(currentUser);
        }

        internal static void AssertProjectOverview(
            GetOverviewProjectResult outcome,
            ProjectAggregate project)
        {
            outcome.Description.Should().Be(project.Description);
        }
    }
}
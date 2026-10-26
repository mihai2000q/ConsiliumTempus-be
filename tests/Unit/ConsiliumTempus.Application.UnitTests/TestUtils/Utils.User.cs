using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class User
    {
        internal static void AssertFromRegisterCommand(
            UserAggregate user,
            RegisterCommand command,
            string password)
        {
            user.Id.Should().NotBeNull();
            user.FirstName.Value.Should().Be(command.FirstName.Capitalize());
            user.LastName.Value.Should().Be(command.LastName.Capitalize());
            user.Credentials.Email.Should().Be(command.Email.ToLower());
            user.Credentials.Password.Should().Be(password);
            if (command.Role is null)
                user.Role.Should().BeNull();
            else
                user.Role!.Value.Should().Be(command.Role);
            user.DateOfBirth.Should().Be(command.DateOfBirth);
            user.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            user.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            user.DomainEvents.Should().HaveCount(1);
            var domainEvent = user.DomainEvents[0];
            domainEvent.Should().BeOfType<UserRegistered>();
            ((UserRegistered)domainEvent).User.Should().Be(user);
        }

        internal static void AssertFromUpdateCurrentCommand(
            UserAggregate user,
            UpdateCurrentUserCommand command)
        {
            user.FirstName.Value.Should().Be(command.FirstName);
            user.LastName.Value.Should().Be(command.LastName);
            if (command.Role is null)
                user.Role.Should().BeNull();
            else
                user.Role!.Value.Should().Be(command.Role);
            user.DateOfBirth.Should().Be(command.DateOfBirth);
            user.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertFromUserDeleted(
            UserAggregate user,
            List<WorkspaceAggregate> ownedWorkspaces,
            Dictionary<WorkspaceAggregate, Dictionary<UserAggregate, WorkspaceRole>> previousRoles,
            List<ProjectAggregate> ownedProjects,
            Dictionary<ProjectAggregate, List<UserAggregate>> previousAllowedMembers)
        {
            // Update Owned Workspaces
            ownedWorkspaces.Should().AllSatisfyOrEmpty(w =>
            {
                var roles = previousRoles[w];
                w.Owner.Should().NotBe(user);
                w.IsPersonal.Value.Should().BeFalse();

                roles[w.Owner].Id.Should().Be(roles.Where(r => r.Key != user).Max(r => r.Value.Id));
                var newOwnerMembership = w.Memberships.Single(m => m.User == w.Owner);
                newOwnerMembership.WorkspaceRole.Should().Be(WorkspaceRole.Admin);
                if (roles[w.Owner] != WorkspaceRole.Admin)
                    newOwnerMembership.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

                w.Memberships
                    .Where(m => m.User != w.Owner)
                    .Should()
                    .AllSatisfy(m => m.WorkspaceRole.Should().Be(roles[m.User]));
            });

            // Update Owned Projects
            ownedProjects.Should().AllSatisfyOrEmpty(p =>
            {
                p.Owner.Should().NotBe(user);
                p.AllowedMembers.Should().HaveCountGreaterThan(1).And.Contain(p.Owner);

                if (p.IsPrivate.Value)
                {
                    previousAllowedMembers[p].Should().Contain(p.Owner);
                    return;
                }

                var candidates = p.Workspace.Memberships.Where(m => m.User != user).ToList();
                candidates.Single(m => m.User == p.Owner).WorkspaceRole.Id
                    .Should().Be(candidates.Max(m => m.WorkspaceRole.Id));
            });
        }

        internal static bool AssertFromUserRegistered(UserAggregate user, WorkspaceAggregate workspace)
        {
            workspace.Memberships.Should().HaveCount(1);
            var membership = workspace.Memberships[0];
            membership.Id.Should().Be((user.Id, membership.Workspace.Id));
            membership.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            membership.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            membership.User.Should().Be(user);
            membership.WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            membership.DomainEvents.Should().BeEmpty();

            membership.Workspace.Id.Value.Should().NotBeEmpty();
            membership.Workspace.Name.Value.Should().Be(Constants.Workspace.Name);
            membership.Workspace.Description.Value.Should().Be(Constants.Workspace.Description);
            membership.Workspace.Owner.Should().Be(user);
            membership.Workspace.IsPersonal.Value.Should().Be(true);
            membership.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            membership.Workspace.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            membership.Workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            return true;
        }

        internal static void AssertUser(UserAggregate outcome, UserAggregate expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Credentials.Should().Be(expected.Credentials);
            outcome.FirstName.Should().Be(expected.FirstName);
            outcome.LastName.Should().Be(expected.LastName);
            outcome.Role.Should().Be(expected.Role);
            outcome.DateOfBirth.Should().Be(expected.DateOfBirth);
            outcome.CreatedDateTime.Should().BeCloseTo(expected.CreatedDateTime, TimeSpanPrecision);
            outcome.UpdatedDateTime.Should().BeCloseTo(expected.UpdatedDateTime, TimeSpanPrecision);
            outcome.Memberships.Should().BeEquivalentTo(expected.Memberships);
        }
    }
}
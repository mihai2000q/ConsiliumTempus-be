using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.DeleteCurrent;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.DeleteCurrent;

[Collection(nameof(UserControllerCollection))]
public class UserControllerDeleteCurrentTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new UserData())
{
    [Fact]
    public async Task DeleteCurrentUser_WhenIsSuccessful_ShouldDeleteUserRelatedDataAndReturnSuccessResponse()
    {
        // Arrange
        var user = UserData.Users.First();

        var emptyWorkspaces = user.Memberships
            .Select(m => m.Workspace)
            .Where(w => w.Memberships.Count == 1)
            .ToList();
        var emptyProjects = user.Memberships
            .SelectMany(m => m.Workspace.Projects)
            .Where(p => p.Workspace.Memberships.Count == 1 || (p.IsPrivate.Value && p.AllowedMembers.Count == 1))
            .ToList();
        var emptyTasks = UserData.ProjectTasks
            .Where(t => emptyProjects.Contains(t.Stage.Sprint.Project))
            .ToList();
        var emptySetups = UserData.CustomFieldSetups
            .Where(s => s.Projects.All(emptyProjects.Contains))
            .ToList();
        var preservedWorkspaceIds = UserData.Workspaces
            .Where(w => w.Owner == user && !emptyWorkspaces.Contains(w))
            .Select(w => w.Id)
            .ToList();
        var preservedProjectIds = UserData.Projects
            .Where(p => p.Owner == user && !emptyProjects.Contains(p))
            .Select(p => p.Id)
            .ToList();

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete("api/users/current");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteCurrentUserResponse>();
        response!.Message.Should().Be("Current user has been deleted successfully!");

        // Assert user deleted
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(UserData.Users.Length - 1);
        (await dbContext.Users.FindAsync(user.Id))
            .Should().BeNull();

        // Assert User Deleted Domain Event
        // Assert Owned Workspaces
        dbContext.Workspaces.Should().HaveCount(UserData.Workspaces.Length - emptyWorkspaces.Count);
        dbContext.Workspaces
            .Include(w => w.Memberships)
            .Where(w => preservedWorkspaceIds.Contains(w.Id))
            .ToList()
            .Should().AllSatisfy(w =>
            {
                w.Owner.Should().NotBe(user);
                w.IsPersonal.Value.Should().BeFalse();
                w.Memberships
                    .Single(m => m.User == w.Owner)
                    .WorkspaceRole
                    .Should().Be(WorkspaceRole.Admin);
            });

        // Assert Memberships
        dbContext.Set<Membership>()
            .Where(m => m.User == user)
            .ToList()
            .Should().BeEmpty();

        // Assert Owned Projects
        dbContext.Projects.Should().HaveCount(UserData.Projects.Length - emptyProjects.Count);
        dbContext.Projects
            .Include(p => p.Workspace.Memberships)
            .Include(p => p.AllowedMembers)
            .Where(p => preservedProjectIds.Contains(p.Id))
            .ToList()
            .Should().AllSatisfy(p =>
            {
                p.Owner.Should().NotBe(user);
                p.AllowedMembers.Should().NotBeEmpty();

                var newOwner = p.IsPrivate.Value
                    ? p.AllowedMembers.First(u => u != user)
                    : p.Workspace.Memberships
                        .OrderByDescending(m => m.WorkspaceRole.Id)
                        .First(m => m.User != user)
                        .User;
                p.Owner.Should().Be(newOwner);
                p.AllowedMembers.Should().Contain(newOwner);
            });

        // Assert Project Deleted (the custom field setups of the deleted projects are deleted with them)
        dbContext.CustomFieldSetups.Should().HaveCount(UserData.CustomFieldSetups.Length - emptySetups.Count);

        // Assert Tasks (the ones of the deleted projects are deleted with them)
        dbContext.ProjectTasks.Should().HaveCount(UserData.ProjectTasks.Length - emptyTasks.Count);
        dbContext.ProjectTasks
            .Where(t =>
                t.Assignee == user ||
                t.Reviewer == user ||
                t.Comments.Any(c => c.CreatedBy == user))
            .ToList()
            .Should().BeEmpty();
        dbContext.Set<PeopleCustomField>()
            .Where(cf => cf.Person == user)
            .ToList()
            .Should().BeEmpty();

        // Assert Audits
        AssertNoAuditsOf<CustomFieldSetupAggregate>(dbContext, user);
        AssertNoAuditsOf<Membership>(dbContext, user);
        AssertNoAuditsOf<ProjectSprintAggregate>(dbContext, user);
        AssertNoAuditsOf<ProjectStage>(dbContext, user);
        AssertNoAuditsOf<ProjectStatus>(dbContext, user);
        AssertNoAuditsOf<ProjectTaskAggregate>(dbContext, user);
        AssertNoAuditsOf<ProjectAggregate>(dbContext, user);
        AssertNoAuditsOf<WorkspaceAggregate>(dbContext, user);

        // Assert Invitations
        dbContext.Set<WorkspaceInvitation>()
            .Where(w => w.Sender == user || w.Collaborator == user)
            .ToList()
            .Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteCurrentUser_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange

        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Delete("api/users/current");

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(UserData.Users.Length);
    }

    private static void AssertNoAuditsOf<TAudit>(DbContext dbContext, UserAggregate user)
        where TAudit : class, IAudit
    {
        dbContext.Set<TAudit>()
            .Where(a => a.Audit.CreatedBy == user || a.Audit.UpdatedBy == user)
            .ToList()
            .Should().BeEmpty();
        dbContext.Set<TAudit>()
            .Where(a => a.Audit.CreatedBy == null || a.Audit.UpdatedBy == null)
            .ToList()
            .Should().NotBeEmpty();
    }
}
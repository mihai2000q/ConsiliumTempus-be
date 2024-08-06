using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.DeleteCurrent;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
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
        var emptyWorkspaces = user.Memberships
            .Select(m => m.Workspace)
            .Where(w => w.Memberships.Count == 1)
            .ToList();
        dbContext.Workspaces.Should().HaveCount(UserData.Workspaces.Length - emptyWorkspaces.Count);

        var preservedWorkspaces = dbContext.Workspaces
            .Include(w => w.Memberships)
            .Where(w => w.Owner == user)
            .ToList();

        preservedWorkspaces
            .Should()
            .AllSatisfy(w =>
            {
                w.Owner.Should().NotBe(user);
                w.IsPersonal.Value.Should().BeFalse();
                w.Memberships
                    .Single(m => m.User == w.Owner)
                    .WorkspaceRole
                    .Should().Be(WorkspaceRole.Admin);
            });

        // Assert Owned Projects
        var emptyProjects = user.Memberships
            .SelectMany(m => m.Workspace.Projects)
            .Where(p => p.Workspace.Memberships.Count == 1 || (p.IsPrivate.Value && p.AllowedMembers.Count == 1))
            .ToList();
        dbContext.Projects.Should().HaveCount(UserData.Projects.Length - emptyProjects.Count);
        
        var preservedProjects = dbContext.Projects
            .Include(p => p.Workspace.Memberships)
            .Include(p => p.AllowedMembers)
            .Where(p => p.Owner == user)
            .ToList();

        preservedProjects.Should().AllSatisfy(p =>
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
        
        dbContext.Set<Audit>()
            .Where(a => a.CreatedBy == null || a.UpdatedBy == null)
            .Should().NotBeEmpty();

        dbContext.Set<WorkspaceInvitation>()
            .Where(w => w.Sender == user || w.Collaborator == user)
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
}
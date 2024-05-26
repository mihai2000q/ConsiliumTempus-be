using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.DeleteCurrent;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
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

        // assert user deleted
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(UserData.Users.Length - 1);
        (await dbContext.Users.FindAsync(user.Id))
            .Should().BeNull();

        // assert user deleted event
        // assert 
        var emptyWorkspaces = user.Memberships
            .Select(m => m.Workspace)
            .Where(w => w.Memberships.Count == 1)
            .ToList();
        dbContext.Workspaces.Should().HaveCount(UserData.Workspaces.Length - emptyWorkspaces.Count);

        var preservedWorkspaces = user.Memberships
            .Select(m => m.Workspace)
            .Where(w => w.Memberships.Count > 1)
            .ToList();
        var newPreservedWorkspaces = dbContext.Workspaces
            .Include(w => w.Memberships)
            .ThenInclude(m => m.User)
            .Include(w => w.Owner)
            .Where(w => preservedWorkspaces.Contains(w))
            .ToList();

        newPreservedWorkspaces
            .Should()
            .AllSatisfy(w =>
            {
                w.Owner.Should().NotBe(user);
                w.IsPersonal.Value.Should().BeFalse();

                var oldWorkspace = preservedWorkspaces.Single(x => x.Id == w.Id);
                var newOwnerAdmin = oldWorkspace.Memberships
                    .FirstOrDefault(m => m.WorkspaceRole.Equals(WorkspaceRole.Admin) && m.User != user);
                if (newOwnerAdmin is not null)
                {
                    w.Owner.Id.Should().Be(newOwnerAdmin.User.Id);
                }
                else
                {
                    var newOwnerMembership = oldWorkspace.Memberships.First(m => m.User != user);
                    newOwnerMembership.WorkspaceRole.Should().NotBe(WorkspaceRole.Admin);
                    var newOwner = w.Memberships.Single(m => m.Id == newOwnerMembership.Id);
                    newOwner.WorkspaceRole.Should().Be(WorkspaceRole.Admin);
                    newOwner.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
                }
            });
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
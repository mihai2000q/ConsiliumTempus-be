using ConsiliumTempus.Domain.Common.ValueObjects;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Leave;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Leave;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerLeaveTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task LeaveWorkspace_WhenIsSuccessful_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var user = WorkspaceData.Users[3];
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest(workspace.Id.Value);

        var projectToBeRemoved = WorkspaceData.Projects[^1];

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/workspaces/{request.Id}/leave");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<LeaveWorkspaceResponse>();
        response!.Message.Should().Be("Workspace has been left successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();

        dbContext.Projects.SingleOrDefault(p => p.Id == projectToBeRemoved.Id)
            .Should().BeNull();

        var updatedWorkspace = await dbContext.Workspaces
            .Include(w => w.Projects)
            .ThenInclude(p => p.AllowedMembers)
            .Include(w => w.Projects)
            .ThenInclude(p => p.Favorites)
            .Include(w => w.Favorites)
            .Include(w => w.Memberships)
            .SingleAsync(w => w.Id == WorkspaceId.Create(request.Id));
        Utils.Workspace.AssertLeave(request, updatedWorkspace, user);
    }

    [Fact]
    public async Task LeaveWorkspace_WhenUserOwnsPrivateProject_ShouldMakeTheNewOwnerAnAllowedMember()
    {
        // Arrange
        var user = WorkspaceData.Users[3];
        var workspace = WorkspaceData.Workspaces.First();
        var nextOwner = WorkspaceData.Users[5]; // the admin that is not the owner of the workspace
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest(workspace.Id.Value);

        var projectId = WorkspaceData.Projects[2].Id;
        await using (var arrangeDbContext = await DbContextFactory.CreateDbContextAsync())
        {
            var project = await arrangeDbContext.Projects
                .Include(p => p.AllowedMembers)
                .SingleAsync(p => p.Id == projectId);
            var userToOwn = await arrangeDbContext.Users.SingleAsync(u => u.Id == user.Id);
            var currentOwner = project.Owner;
            project.UpdatePrivacy(IsPrivate.Create(true));
            project.UpdateOwner(userToOwn);
            if (!project.AllowedMembers.Contains(currentOwner)) project.AddAllowedMember(currentOwner);
            if (!project.AllowedMembers.Contains(userToOwn)) project.AddAllowedMember(userToOwn);
            project.AllowedMembers.Should().NotContain(u => u.Id == nextOwner.Id);
            await arrangeDbContext.SaveChangesAsync();
        }

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/workspaces/{request.Id}/leave");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedProject = await dbContext.Projects
            .Include(p => p.AllowedMembers)
            .SingleAsync(p => p.Id == projectId);
        updatedProject.Owner.Id.Should().Be(nextOwner.Id);
        updatedProject.AllowedMembers.Should().Contain(u => u.Id == nextOwner.Id);
        updatedProject.AllowedMembers.Should().NotContain(u => u.Id == user.Id);
        updatedProject.AllowedMembers.Count(u => u.Id == nextOwner.Id).Should().Be(1);
    }

    [Fact]
    public async Task LeaveWorkspace_WhenUserIsOwner_ShouldReturnLeaveOwnedWorkspaceError()
    {
        // Arrange
        var user = WorkspaceData.Users[0];
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest(workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/workspaces/{request.Id}/leave");

        // Assert
        await outcome.ValidateError(Errors.Workspace.LeaveOwned);

        workspace.Owner.Should().Be(user);
    }

    [Fact]
    public async Task LeaveWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest();

        // Act
        var outcome = await Client.Delete($"api/workspaces/{request.Id}/leave");

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.SingleOrDefault(w => w.Id == WorkspaceId.Create(request.Id))
            .Should().BeNull();
    }
}
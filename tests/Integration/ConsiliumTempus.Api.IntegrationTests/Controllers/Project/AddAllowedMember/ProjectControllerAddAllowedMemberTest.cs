using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.AddAllowedMember;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.AddAllowedMember;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerAddAllowedMemberTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task AddAllowedMemberToProject_WhenSucceeds_ShouldAddAllowedMemberToProjectAndReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects[^1];
        var collaborator = ProjectData.Users[4];
        var request = ProjectRequestFactory.CreateAddAllowedMemberToProjectRequest(
            project.Id.Value,
            collaborator.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Post("api/projects/Add-Allowed-Member", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<AddAllowedMemberToProjectResponse>();
        response!.Message.Should().Be("Allowed member has been added to project successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedProject = await dbContext.Projects
            .Include(p => p.AllowedMembers)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertAddAllowedMember(updatedProject, request, collaborator);
    }

    [Fact]
    public async Task AddAllowedMemberToProject_WhenIsAlreadyAllowedMember_ShouldReturnAlreadyAllowedMemberError()
    {
        // Arrange
        var project = ProjectData.Projects[^1];
        var collaborator = project.Owner;
        var request = ProjectRequestFactory.CreateAddAllowedMemberToProjectRequest(
            project.Id.Value,
            collaborator.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Post("api/projects/Add-Allowed-Member", request);

        // Assert
        await outcome.ValidateError(Errors.Project.AlreadyAllowedMember);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects
            .Include(p => p.AllowedMembers)
            .Single(p => p.Id == ProjectId.Create(request.Id))
            .AllowedMembers
            .Should().Contain(collaborator);
    }

    [Fact]
    public async Task AddAllowedMemberToProject_WhenCollaboratorIsNull_ShouldReturnCollaboratorNotFoundError()
    {
        // Arrange
        var project = ProjectData.Projects[^1];
        var request = ProjectRequestFactory.CreateAddAllowedMemberToProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Post("api/projects/Add-Allowed-Member", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.CollaboratorNotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects
            .Include(p => p.Workspace.Memberships)
            .Single(p => p.Id == ProjectId.Create(request.Id))
            .Workspace
            .Memberships
            .Should().NotContain(m => m.User.Id.Value == request.CollaboratorId);
    }
    
    [Fact]
    public async Task AddAllowedMemberToProject_WhenIsNotPrivate_ShouldReturnNotPrivateError()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateAddAllowedMemberToProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Post("api/projects/Add-Allowed-Member", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotPrivate);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Single(p => p.Id == ProjectId.Create(request.Id))
            .IsPrivate.Value
            .Should().BeFalse();
    }
    
    [Fact]
    public async Task AddAllowedMemberToProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateAddAllowedMemberToProjectRequest();

        // Act
        var outcome = await Client.Post("api/projects/Add-Allowed-Member", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id))
            .Should().BeNull();
    }
}
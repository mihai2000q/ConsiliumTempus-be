using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.RemoveAllowedMember;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.RemoveAllowedMember;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerRemoveAllowedMemberTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task RemoveAllowedMemberFromProject_WhenSucceeds_ShouldRemoveAllowedMemberFromProjectAndReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects[^2];
        var user = project.Owner;
        var allowedMember = project.AllowedMembers.First(u => u != user);
        var request = ProjectRequestFactory.CreateRemoveAllowedMemberFromProjectRequest(
            project.Id.Value,
            allowedMember.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/projects/" +
                                          $"{request.Id}/Remove-Allowed-Member/{request.AllowedMemberId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<RemoveAllowedMemberFromProjectResponse>();
        response!.Message.Should().Be("Allowed Member has been removed from project successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedProject = await dbContext.Projects
            .Include(p => p.AllowedMembers)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertRemoveAllowedMember(updatedProject, request);
    }

    [Fact]
    public async Task RemoveAllowedMemberFromProject_WhenRequestHasCurrentUserAsAllowedMember_ShouldReturnRemoveYourselfError()
    {
        // Arrange
        var project = ProjectData.Projects[^2];
        var user = project.AllowedMembers[0];
        var request = ProjectRequestFactory.CreateRemoveAllowedMemberFromProjectRequest(
            project.Id.Value,
            user.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/projects/" +
                                          $"{request.Id}/Remove-Allowed-Member/{request.AllowedMemberId}");

        // Assert
        await outcome.ValidateError(Errors.Project.RemoveYourself);
    }

    [Fact]
    public async Task RemoveAllowedMemberFromProject_WhenAllowedMemberIsNull_ShouldReturnAllowedMemberNotFoundError()
    {
        // Arrange
        var project = ProjectData.Projects[^1];
        var request = ProjectRequestFactory.CreateRemoveAllowedMemberFromProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Delete($"api/projects/" +
                                          $"{request.Id}/Remove-Allowed-Member/{request.AllowedMemberId}");

        // Assert
        await outcome.ValidateError(Errors.Project.AllowedMemberNotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects
            .Include(p => p.AllowedMembers)
            .Single(p => p.Id == ProjectId.Create(request.Id))
            .AllowedMembers
            .Should().NotContain(u => u.Id.Value == request.AllowedMemberId);
    }
    
    [Fact]
    public async Task RemoveAllowedMemberFromProject_WhenIsNotPrivate_ShouldReturnNotPrivateError()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateRemoveAllowedMemberFromProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Delete($"api/projects/" +
                                          $"{request.Id}/Remove-Allowed-Member/{request.AllowedMemberId}");

        // Assert
        await outcome.ValidateError(Errors.Project.NotPrivate);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Single(p => p.Id == ProjectId.Create(request.Id))
            .IsPrivate.Value
            .Should().BeFalse();
    }
    
    [Fact]
    public async Task RemoveAllowedMemberFromProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateRemoveAllowedMemberFromProjectRequest();

        // Act
        var outcome = await Client.Delete($"api/projects/" +
                                          $"{request.Id}/Remove-Allowed-Member/{request.AllowedMemberId}");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id))
            .Should().BeNull();
    }
}
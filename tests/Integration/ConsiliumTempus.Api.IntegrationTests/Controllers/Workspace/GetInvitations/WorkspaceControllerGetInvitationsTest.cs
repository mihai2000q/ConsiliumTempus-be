using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.GetInvitations;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.GetInvitations;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerGetInvitationsTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task GetInvitationsWorkspace_WhenRequestHasIsSenderTrue_ShouldReturnInvitationsForSender()
    {
        // Arrange
        var user = WorkspaceData.Users[0];
        var request = WorkspaceRequestFactory.CreateGetInvitationsWorkspaceRequest(isSender: true);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/workspaces/invitations?isSender={request.IsSender}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetInvitationsWorkspaceResponse>();

        var expectedInvitations = WorkspaceData.WorkspaceInvitations
            .Where(i => i.Sender == user)
            .ToList();

        Utils.Workspace.AssertGetInvitationsResponse(
            response!,
            expectedInvitations,
            expectedInvitations.Count);
    }

    [Fact]
    public async Task GetInvitationsWorkspace_WhenRequestHasIsSenderFalse_ShouldReturnInvitationsForCollaborator()
    {
        // Arrange
        var user = WorkspaceData.Users[7];
        var request = WorkspaceRequestFactory.CreateGetInvitationsWorkspaceRequest(isSender: false);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/workspaces/invitations?isSender={request.IsSender}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetInvitationsWorkspaceResponse>();

        var expectedInvitations = WorkspaceData.WorkspaceInvitations
            .Where(i => i.Collaborator == user)
            .ToList();

        Utils.Workspace.AssertGetInvitationsResponse(
            response!,
            expectedInvitations,
            expectedInvitations.Count);
    }

    [Fact]
    public async Task GetInvitationsWorkspace_WhenRequestHasWorkspaceId_ShouldReturnInvitationsForWorkspace()
    {
        // Arrange
        var user = WorkspaceData.Users[0];
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateGetInvitationsWorkspaceRequest(workspaceId: workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/workspaces/invitations?workspaceId={request.WorkspaceId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetInvitationsWorkspaceResponse>();

        Utils.Workspace.AssertGetInvitationsResponse(
            response!,
            workspace.Invitations,
            workspace.Invitations.Count);
    }

    [Fact]
    public async Task GetInvitationsWorkspace_WhenRequestHasIsSenderFalseAndPaginationInfo_ShouldReturnPaginatedInvitationsForCollaborator()
    {
        // Arrange
        var user = WorkspaceData.Users[7];
        var request = WorkspaceRequestFactory.CreateGetInvitationsWorkspaceRequest(
            isSender: false,
            pageSize: 2,
            currentPage: 1);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/workspaces/invitations" +
                                       $"?isSender={request.IsSender}" +
                                       $"&pageSize={request.PageSize}" +
                                       $"&currentPage={request.CurrentPage}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetInvitationsWorkspaceResponse>();

        var expectedInvitations = WorkspaceData.WorkspaceInvitations
            .Where(i => i.Collaborator == user)
            .OrderByDescending(i => i.CreatedDateTime)
            .Skip(request.PageSize!.Value * (request.CurrentPage!.Value - 1))
            .Take(request.PageSize!.Value)
            .ToList();

        var expectedInvitationsCount = WorkspaceData.WorkspaceInvitations
            .Count(i => i.Collaborator == user);

        Utils.Workspace.AssertGetInvitationsResponse(
            response!,
            expectedInvitations,
            expectedInvitationsCount);
    }
}
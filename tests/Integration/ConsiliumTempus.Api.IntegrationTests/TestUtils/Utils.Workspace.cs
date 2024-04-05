using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Domain.Workspace;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class Workspace
    {
        internal static async Task AssertDtoFromResponse(
            HttpResponseMessage response,
            WorkspaceAggregate workspace)
        {
            var dto = await response.Content.ReadFromJsonAsync<WorkspaceDto>();
            AssertDto(dto!, workspace);
        }

        internal static void AssertDto(
            WorkspaceDto dto,
            WorkspaceAggregate workspace)
        {
            dto.Id.Should().Be(workspace.Id.ToString());
            dto.Name.Should().Be(workspace.Name.Value);
            dto.Description.Should().Be(workspace.Description.Value);
        }

        internal static void AssertCreation(WorkspaceAggregate workspace, CreateWorkspaceRequest request)
        {
            workspace.Id.Value.Should().NotBeEmpty();
            workspace.Name.Value.Should().Be(request.Name);
            workspace.Description.Value.Should().Be(request.Description);
        }

        internal static void AssertUpdated(
            WorkspaceAggregate workspace, 
            WorkspaceAggregate newWorkspace, 
            UpdateWorkspaceRequest request)
        {
            // unchanged
            newWorkspace.CreatedDateTime.Should().Be(workspace.CreatedDateTime);
            
            // changed
            newWorkspace.Id.Value.Should().Be(request.Id);
            newWorkspace.Name.Value.Should().Be(request.Name);
            newWorkspace.Description.Value.Should().Be(request.Description);
            newWorkspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }
    }
}
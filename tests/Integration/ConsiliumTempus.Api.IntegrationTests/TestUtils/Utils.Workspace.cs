using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Domain.Workspace;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class Workspace
    {
        internal static void AssertGetResponse(
            GetWorkspaceResponse response,
            WorkspaceAggregate workspace)
        {
            response.Name.Should().Be(workspace.Name.Value);
            response.Description.Should().Be(workspace.Description.Value);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionWorkspaceResponse response,
            List<WorkspaceAggregate> workspaces)
        {
            response.Workspaces.Should().HaveCount(workspaces.Count);
            response.Workspaces
                .OrderBy(c => c.Name)
                .Zip(workspaces)
                .Should().AllSatisfy(x => AssertWorkspaceResponse(x.First, x.Second));
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
        
        private static void AssertWorkspaceResponse(
            GetCollectionWorkspaceResponse.WorkspaceResponse response,
            WorkspaceAggregate workspace)
        {
            response.Id.Should().Be(workspace.Id.ToString());
            response.Name.Should().Be(workspace.Name.Value);
            response.Description.Should().Be(workspace.Description.Value);
        }
    }
}
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
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

        internal static void AssertCreation(
            WorkspaceAggregate workspace, 
            CreateWorkspaceRequest request,
            UserAggregate user)
        {
            workspace.Id.Value.Should().NotBeEmpty();
            workspace.Name.Value.Should().Be(request.Name);
            workspace.Description.Value.Should().Be(request.Description);
            workspace.Owner.Should().Be(user);
            workspace.IsUserWorkspace.Value.Should().Be(false);
            workspace.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

            workspace.Memberships.Should().HaveCount(1);
            workspace.Memberships[0].User.Should().Be(user);
            workspace.Memberships[0].WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            workspace.Memberships[0].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            workspace.Memberships[0].UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
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
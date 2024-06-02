using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

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
            response.IsFavorite.Should().Be(workspace.IsFavorite.Value);
            response.IsPersonal.Should().Be(workspace.IsPersonal.Value);
            response.Description.Should().Be(workspace.Description.Value);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionWorkspaceResponse response,
            List<WorkspaceAggregate> workspaces,
            int totalCount,
            int? totalPages,
            bool isOrdered = false)
        {
            response.Workspaces.Should().HaveCount(workspaces.Count);
            if (isOrdered)
            {
                response.Workspaces.Zip(workspaces)
                    .Should().AllSatisfy(x => AssertWorkspaceResponse(x.First, x.Second));
            }
            else
            {
                response.Workspaces
                    .OrderBy(w => w.Id)
                    .Zip(workspaces.OrderBy(w => w.Id.Value))
                    .Should().AllSatisfy(x => AssertWorkspaceResponse(x.First, x.Second));
            }

            response.TotalCount.Should().Be(totalCount);
        }

        internal static void AssertCreation(
            WorkspaceAggregate workspace,
            CreateWorkspaceRequest request,
            UserAggregate user)
        {
            workspace.Id.Value.Should().NotBeEmpty();
            workspace.Name.Value.Should().Be(request.Name);
            workspace.Description.Value.Should().BeEmpty();
            workspace.Owner.Should().Be(user);
            workspace.IsPersonal.Value.Should().Be(false);
            workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            workspace.Memberships.Should().HaveCount(1);
            workspace.Memberships[0].User.Should().Be(user);
            workspace.Memberships[0].WorkspaceRole.Should().Be(WorkspaceRole.Admin);
            workspace.Memberships[0].CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            workspace.Memberships[0].UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdated(
            WorkspaceAggregate workspace,
            WorkspaceAggregate newWorkspace,
            UpdateWorkspaceRequest request)
        {
            // unchanged
            newWorkspace.Id.Value.Should().Be(request.Id);
            newWorkspace.CreatedDateTime.Should().Be(workspace.CreatedDateTime);

            // changed
            newWorkspace.Name.Value.Should().Be(request.Name);
            newWorkspace.Description.Value.Should().Be(request.Description);
            newWorkspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newWorkspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        private static void AssertWorkspaceResponse(
            GetCollectionWorkspaceResponse.WorkspaceResponse response,
            WorkspaceAggregate workspace)
        {
            response.Id.Should().Be(workspace.Id.ToString());
            response.Name.Should().Be(workspace.Name.Value);
            response.Description.Should().Be(workspace.Description.Value);
            response.IsFavorite.Should().Be(workspace.IsFavorite.Value);
            response.IsPersonal.Should().Be(workspace.IsPersonal.Value);

            var owner = workspace.Owner;
            response.Owner.Id.Should().Be(owner.Id.Value);
            response.Owner.Name.Should().Be(owner.FirstName + " " + owner.LastName);
        }
    }
}
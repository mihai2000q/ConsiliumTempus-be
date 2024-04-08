using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Domain.Workspace;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Workspace
    {
        internal static bool AssertGetQuery(
            GetWorkspaceQuery query,
            GetWorkspaceRequest request)
        {
            query.Id.Should().Be(request.Id);
            return true;
        }

        internal static bool AssertGetCollectionQuery(GetCollectionWorkspaceQuery query)
        {
            query.Should().Be(new GetCollectionWorkspaceQuery());
            return true;
        }

        internal static bool AssertCreateCommand(CreateWorkspaceCommand command, CreateWorkspaceRequest request)
        {
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            return true;
        }

        internal static bool AssertUpdateCommand(
            UpdateWorkspaceCommand command,
            UpdateWorkspaceRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            return true;
        }

        internal static void AssertGetResponse(
            IActionResult outcome,
            WorkspaceAggregate workspace)
        {
            outcome.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)outcome).Value.Should().BeOfType<GetWorkspaceResponse>();

            var response = ((OkObjectResult)outcome).Value as GetWorkspaceResponse;
            
            response!.Name.Should().Be(workspace.Name.Value);
            response.Description.Should().Be(workspace.Description.Value);
        }
        
        internal static void AssertGetCollectionResponse(
            IActionResult outcome,
            GetCollectionWorkspaceResult result)
        {
            outcome.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)outcome).Value.Should().BeOfType<GetCollectionWorkspaceResponse>();

            var response = ((OkObjectResult)outcome).Value as GetCollectionWorkspaceResponse;
            
            response!.Workspaces
                .Zip(result.Workspaces)
                .Should().AllSatisfy(p => AssertWorkspaceResponse(p.First, p.Second));
        }

        private static void AssertWorkspaceResponse(
            GetCollectionWorkspaceResponse.WorkspaceResponse response,
            WorkspaceAggregate workspace)
        {
            response.Id.Should().Be(workspace.Id.Value.ToString());
            response.Name.Should().Be(workspace.Name.Value);
            response.Description.Should().Be(workspace.Description.Value);
        }
    }
}
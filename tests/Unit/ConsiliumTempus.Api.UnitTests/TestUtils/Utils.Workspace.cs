using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.Get;
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
        
        internal static bool AssertCreateCommand(
            CreateWorkspaceCommand command,
            CreateWorkspaceRequest request,
            string token)
        {
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.Token.Should().Be(token);
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

        internal static void AssertDto(
            IActionResult outcome,
            WorkspaceAggregate workspace)
        {
            outcome.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)outcome).Value.Should().BeOfType<WorkspaceDto>();

            var response = ((OkObjectResult)outcome).Value as WorkspaceDto;

            response!.Id.Should().Be(workspace.Id.Value.ToString());
            response.Name.Should().Be(workspace.Name);
            response.Description.Should().Be(workspace.Description);
        }
    }
}
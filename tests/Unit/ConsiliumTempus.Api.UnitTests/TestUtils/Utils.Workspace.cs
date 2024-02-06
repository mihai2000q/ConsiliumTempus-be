using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Domain.Workspace;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

public static partial class Utils
{
    public static class Workspace
    {
        public static bool AssertCreateCommand(
            CreateWorkspaceCommand command, 
            CreateWorkspaceRequest request, 
            string token)
        {
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.Token.Should().Be(token);
            return true;
        }

        public static void AssertDto(
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
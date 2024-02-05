using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Application.Workspace.Commands.Create;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

public static partial class Utils
{
    public static class Workspace
    {
        public static bool AssertCreateCommand(
            WorkspaceCreateCommand command, 
            WorkspaceCreateRequest request, 
            string token)
        {
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.Token.Should().Be(token);
            return true;
        }

        public static void AssertCreateResponse(
            WorkspaceCreateResponse response,
            WorkspaceCreateResult result)
        {
            response.Id.Should().Be(result.Workspace.Id.Value.ToString());
            response.Name.Should().Be(result.Workspace.Name);
            response.Description.Should().Be(result.Workspace.Description);
        }
    }
}
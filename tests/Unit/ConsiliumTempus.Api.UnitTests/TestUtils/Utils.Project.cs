using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Project
    {
        internal static bool AssertCreateCommand(CreateProjectCommand command, CreateProjectRequest request, string token)
        {
            command.WorkspaceId.Should().Be(request.WorkspaceId);
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.IsPrivate.Should().Be(request.IsPrivate);
            command.Token.Should().Be(token);
            return true;
        }

        internal static bool AssertDeleteCommand(DeleteProjectCommand command, Guid id)
        {
            command.Id.Should().Be(id);
            return true;
        }
    }
}
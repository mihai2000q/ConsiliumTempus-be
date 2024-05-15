using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Update;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectStage
    {
        internal static bool AssertCreateCommand(
            CreateProjectStageCommand command,
            CreateProjectStageRequest request)
        {
            command.ProjectSprintId.Should().Be(request.ProjectSprintId);
            command.Name.Should().Be(request.Name);

            return true;
        }

        internal static bool AssertUpdateCommand(
            UpdateProjectStageCommand command,
            UpdateProjectStageRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);

            return true;
        }

        internal static bool AssertDeleteCommand(
            DeleteProjectStageCommand command,
            DeleteProjectStageRequest request)
        {
            command.Id.Should().Be(request.Id);

            return true;
        }
    }
}
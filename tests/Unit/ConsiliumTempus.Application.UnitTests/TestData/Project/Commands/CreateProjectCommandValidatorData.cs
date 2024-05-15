using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class CreateProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<CreateProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateCreateProjectCommand();
            Add(command);

            command = new CreateProjectCommand(
                Guid.NewGuid(),
                "New Project",
                true);
            Add(command);
        }
    }

    internal class GetInvalidWorkspaceIdCommands : TheoryData<CreateProjectCommand, string, int>
    {
        public GetInvalidWorkspaceIdCommands()
        {
            var command = ProjectCommandFactory.CreateCreateProjectCommand(workspaceId: Guid.Empty);
            Add(command, nameof(command.WorkspaceId), 1);
        }
    }

    internal class GetInvalidNameCommands : TheoryData<CreateProjectCommand, string, int>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectCommandFactory.CreateCreateProjectCommand(name: "");
            Add(command, nameof(command.Name), 1);

            command = ProjectCommandFactory.CreateCreateProjectCommand(
                name: new string('a', PropertiesValidation.Project.NameMaximumLength + 1));
            Add(command, nameof(command.Name), 1);
        }
    }
}
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class CreateProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<CreateProjectCommand>
    {
        public GetValidCommands()
        {
            Add(ProjectCommandFactory.CreateCreateProjectCommand());
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

    internal class GetInvalidDescriptionCommands : TheoryData<CreateProjectCommand, string, int>
    {
        public GetInvalidDescriptionCommands()
        {
            var command = ProjectCommandFactory.CreateCreateProjectCommand(description: "");
            Add(command, nameof(command.Description), 1);

            command = ProjectCommandFactory.CreateCreateProjectCommand(
                description: new string('a', PropertiesValidation.Project.DescriptionMaximumLength + 1));
            Add(command, nameof(command.Description), 1);
        }
    }
}
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Sprint.Commands;

internal static class CreateProjectSprintCommandValidatorData
{
    internal class GetValidCommands : TheoryData<CreateProjectSprintCommand>
    {
        public GetValidCommands()
        {
            Add(ProjectSprintCommandFactory.CreateCreateProjectSprintCommand());
        }
    }

    internal class GetInvalidProjectIdCommands : TheoryData<CreateProjectSprintCommand, string, int>
    {
        public GetInvalidProjectIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(projectId: Guid.Empty);
            Add(command, nameof(command.ProjectId), 1);
        }
    }

    internal class GetInvalidNameCommands : TheoryData<CreateProjectSprintCommand, string, int>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(name: "");
            Add(command, nameof(command.Name), 1);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                name: new string('a', PropertiesValidation.ProjectSprint.NameMaximumLength + 1));
            Add(command, nameof(command.Name), 1);
        }
    }
}
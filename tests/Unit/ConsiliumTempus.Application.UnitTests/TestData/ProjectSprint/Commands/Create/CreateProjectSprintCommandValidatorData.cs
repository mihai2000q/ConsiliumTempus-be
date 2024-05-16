using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.Create;

internal static class CreateProjectSprintCommandValidatorData
{
    internal class GetValidCommands : TheoryData<CreateProjectSprintCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand();
            Add(command);

            command = new CreateProjectSprintCommand(
                Guid.NewGuid(),
                "New Project Sprint",
                new DateOnly(2022, 11, 12),
                new DateOnly(2022, 11, 26));
            Add(command);
        }
    }

    internal class GetInvalidProjectIdCommands : TheoryData<CreateProjectSprintCommand, string>
    {
        public GetInvalidProjectIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(projectId: Guid.Empty);
            Add(command, nameof(command.ProjectId));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<CreateProjectSprintCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(name: "");
            Add(command, nameof(command.Name));

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                name: new string('a', PropertiesValidation.ProjectSprint.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}
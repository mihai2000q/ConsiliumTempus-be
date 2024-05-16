using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.AddStage;

internal static class AddStageToProjectSprintCommandValidatorData
{
    internal class GetValidCommands : TheoryData<AddStageToProjectSprintCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectSprintCommandFactory.CreateAddStageToProjectSprintCommand();
            Add(command);

            command = new AddStageToProjectSprintCommand(
                Guid.NewGuid(),
                "Staging",
                true);
            Add(command);
        }
    }

    internal class GetInvalidProjectSprintIdCommands : TheoryData<AddStageToProjectSprintCommand, string>
    {
        public GetInvalidProjectSprintIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateAddStageToProjectSprintCommand(projectSprintId: Guid.Empty);
            Add(command, nameof(command.ProjectSprintId));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<AddStageToProjectSprintCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectSprintCommandFactory.CreateAddStageToProjectSprintCommand(name: "");
            Add(command, nameof(command.Name));

            command = ProjectSprintCommandFactory.CreateAddStageToProjectSprintCommand(
                name: new string('*', PropertiesValidation.ProjectStage.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}
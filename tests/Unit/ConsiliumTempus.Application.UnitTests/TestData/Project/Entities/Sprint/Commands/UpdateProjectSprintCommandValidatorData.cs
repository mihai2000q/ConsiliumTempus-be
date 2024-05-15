using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Update;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Sprint.Commands;

internal static class UpdateProjectSprintCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateProjectSprintCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectSprintCommandFactory.CreateUpdateProjectSprintCommand();
            Add(command);

            command = new UpdateProjectSprintCommand(
                Guid.NewGuid(),
                "New Sprint",
                new DateOnly(2022, 12, 12),
                new DateOnly(2022, 12, 24));
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateProjectSprintCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateUpdateProjectSprintCommand(
                id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<UpdateProjectSprintCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectSprintCommandFactory.CreateUpdateProjectSprintCommand(
                name: string.Empty);
            Add(command, nameof(command.Name));

            command = ProjectSprintCommandFactory.CreateUpdateProjectSprintCommand(
                name: new string('*', PropertiesValidation.ProjectSprint.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}
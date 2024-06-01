using ConsiliumTempus.Application.Common.Extensions;
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

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                startDate: new DateOnly(2022, 10, 10));
            Add(command);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                endDate: new DateOnly(2022, 10, 10));
            Add(command);

            command = new CreateProjectSprintCommand(
                Guid.NewGuid(),
                "New Project Sprint",
                new DateOnly(2022, 11, 12),
                new DateOnly(2022, 11, 26),
                true);
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

    internal class GetInvalidStartEndDateCommands : TheoryData<CreateProjectSprintCommand, string>
    {
        public GetInvalidStartEndDateCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                startDate: new DateOnly(2022, 10, 10),
                endDate: new DateOnly(2022, 10, 10));
            Add(command, nameof(command.StartDate).And(nameof(command.EndDate)));

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                startDate: new DateOnly(2022, 10, 10),
                endDate: new DateOnly(2022, 10, 9));
            Add(command, nameof(command.StartDate).And(nameof(command.EndDate)));
        }
    }
}
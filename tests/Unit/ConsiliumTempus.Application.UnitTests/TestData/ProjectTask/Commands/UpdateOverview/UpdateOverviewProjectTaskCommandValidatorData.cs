using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateOverview;

internal static class UpdateOverviewProjectTaskCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateOverviewProjectTaskCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateOverviewProjectTaskCommand();
            Add(command);

            command = new UpdateOverviewProjectTaskCommand(
                Guid.NewGuid(),
                "New Project Task Name",
                "New Description",
                null);
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateOverviewProjectTaskCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateOverviewProjectTaskCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<UpdateOverviewProjectTaskCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateOverviewProjectTaskCommand(name: "");
            Add(command, nameof(command.Name));

            command = ProjectTaskCommandFactory.CreateUpdateOverviewProjectTaskCommand(
                name: new string('*', PropertiesValidation.ProjectTask.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}
using ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class UpdatePrivacyProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdatePrivacyProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateUpdatePrivacyProjectCommand();
            Add(command);

            command = new UpdatePrivacyProjectCommand(
                Guid.NewGuid(),
                true);
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdatePrivacyProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateUpdatePrivacyProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}
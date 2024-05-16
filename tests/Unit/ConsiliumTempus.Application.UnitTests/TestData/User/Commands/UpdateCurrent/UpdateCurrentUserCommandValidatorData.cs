using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.User.Commands.UpdateCurrent;

internal static class UpdateCurrentUserCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateCurrentUserCommand>
    {
        public GetValidCommands()
        {
            var command = UserCommandFactory.CreateUpdateUserCommand();
            Add(command);

            command = new UpdateCurrentUserCommand(
                "new Michael",
                "jackson or Jordan?",
                "Pro Singer and Basketball Player",
                new DateOnly(1978, 12, 23));
            Add(command);
        }
    }
    
    internal class GetInvalidFirstNameCommands : TheoryData<UpdateCurrentUserCommand, string>
    {
        public GetInvalidFirstNameCommands()
        {
            var command = UserCommandFactory.CreateUpdateUserCommand(firstName: "");
            Add(command, nameof(command.FirstName));

            command = UserCommandFactory.CreateUpdateUserCommand(
                firstName: new string('a', PropertiesValidation.User.FirstNameMaximumLength + 1));
            Add(command, nameof(command.FirstName));
        }
    }

    internal class GetInvalidLastNameCommands : TheoryData<UpdateCurrentUserCommand, string>
    {
        public GetInvalidLastNameCommands()
        {
            var command = UserCommandFactory.CreateUpdateUserCommand(lastName: "");
            Add(command, nameof(command.LastName));

            command = UserCommandFactory.CreateUpdateUserCommand(
                lastName: new string('a', PropertiesValidation.User.LastNameMaximumLength + 1));
            Add(command, nameof(command.LastName));
        }
    }

    internal class GetInvalidRoleCommands : TheoryData<UpdateCurrentUserCommand, string>
    {
        public GetInvalidRoleCommands()
        {
            var command = UserCommandFactory.CreateUpdateUserCommand(
                role: new string('a', PropertiesValidation.User.RoleMaximumLength + 1));
            Add(command, nameof(command.Role));
        }
    }

    internal class GetInvalidDateOfBirthCommands : TheoryData<UpdateCurrentUserCommand, string>
    {
        public GetInvalidDateOfBirthCommands()
        {
            var command = UserCommandFactory.CreateUpdateUserCommand(
                dateOfBirth: DateOnly.FromDateTime(DateTime.UtcNow));
            Add(command, nameof(command.DateOfBirth));
        }
    }
}
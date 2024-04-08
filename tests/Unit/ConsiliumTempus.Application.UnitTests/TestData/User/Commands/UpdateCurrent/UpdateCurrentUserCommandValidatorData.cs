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
            Add(UserCommandFactory.CreateUpdateUserCommand());
            Add(new UpdateCurrentUserCommand(
                "new Michael",
                "jackson or Jordan?",
                "Pro Singer and Basketball Player",
                new DateOnly(1978, 12, 23)));
        }
    }
    
    internal class GetInvalidFirstNameCommands : TheoryData<UpdateCurrentUserCommand, string, int>
    {
        public GetInvalidFirstNameCommands()
        {
            var command = UserCommandFactory.CreateUpdateUserCommand(firstName: "");
            Add(command, nameof(command.FirstName), 1);

            command = UserCommandFactory.CreateUpdateUserCommand(
                firstName: new string('a', PropertiesValidation.User.FirstNameMaximumLength + 1));
            Add(command, nameof(command.FirstName), 1);
        }
    }

    internal class GetInvalidLastNameCommands : TheoryData<UpdateCurrentUserCommand, string, int>
    {
        public GetInvalidLastNameCommands()
        {
            var command = UserCommandFactory.CreateUpdateUserCommand(lastName: "");
            Add(command, nameof(command.LastName), 1);

            command = UserCommandFactory.CreateUpdateUserCommand(
                lastName: new string('a', PropertiesValidation.User.LastNameMaximumLength + 1));
            Add(command, nameof(command.LastName), 1);
        }
    }

    internal class GetInvalidRoleCommands : TheoryData<UpdateCurrentUserCommand, string, int>
    {
        public GetInvalidRoleCommands()
        {
            var command = UserCommandFactory.CreateUpdateUserCommand(
                role: new string('a', PropertiesValidation.User.RoleMaximumLength + 1));
            Add(command, nameof(command.Role), 1);
        }
    }

    internal class GetInvalidDateOfBirthCommands : TheoryData<UpdateCurrentUserCommand, string, int>
    {
        public GetInvalidDateOfBirthCommands()
        {
            var command = UserCommandFactory.CreateUpdateUserCommand(
                dateOfBirth: DateOnly.FromDateTime(DateTime.UtcNow));
            Add(command, nameof(command.DateOfBirth), 1);
        }
    }
}
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands;

internal static partial class Data
{
    internal static class RegisterCommandValidator
    {
        internal class GetValidCommands : TheoryData<RegisterCommand>
        {
            public GetValidCommands()
            {
                Add(new RegisterCommand(
                    "Michael",
                    "Jordan",
                    "MichaelJ@Gmail.com",
                    "Password123",
                    "Pro Basketball Player",
                    new DateOnly(1991, 5, 23)));
            }
        }

        internal class GetInvalidFirstNameCommands : TheoryData<RegisterCommand, string, int>
        {
            public GetInvalidFirstNameCommands()
            {
                var command = new RegisterCommand(
                    "",
                    "Jordan",
                    "MichaelJ@Gmail.com",
                    "Password123",
                    "Pro Basketball Player",
                    new DateOnly(1991, 5, 23));
                Add(command, nameof(command.FirstName), 1);
            }
        }

        internal class GetInvalidLastNameCommands : TheoryData<RegisterCommand, string, int>
        {
            public GetInvalidLastNameCommands()
            {
                var command = new RegisterCommand(
                    "Michael",
                    "",
                    "MichaelJ@Gmail.com",
                    "Password123",
                    "Pro Basketball Player",
                    new DateOnly(1991, 5, 23));
                Add(command, nameof(command.LastName), 1);
            }
        }

        internal class GetInvalidEmailCommands : TheoryData<RegisterCommand, string, int>
        {
            public GetInvalidEmailCommands()
            {
                var command = new RegisterCommand(
                    "Michael",
                    "Jordan",
                    "",
                    "Password123",
                    "Pro Basketball Player",
                    new DateOnly(1991, 5, 23));
                Add(command, nameof(command.Email), 2);
                command = new RegisterCommand(
                    "Michael",
                    "Jordan",
                    "This is not an email",
                    "Password123",
                    "Pro Basketball Player",
                    new DateOnly(1991, 5, 23));
                Add(command, nameof(command.Email), 1);
            }
        }

        internal class GetInvalidPasswordCommands : TheoryData<RegisterCommand, string, int>
        {
            public GetInvalidPasswordCommands()
            {
                var command = new RegisterCommand(
                    "Michael",
                    "Jordan",
                    "MichaelJ@Gmail.com",
                    "",
                    "Pro Basketball Player",
                    new DateOnly(1991, 5, 23));
                Add(command, nameof(command.Password), 5);
            }
        }

        internal class GetInvalidRoleCommands : TheoryData<RegisterCommand, string, int>
        {
            public GetInvalidRoleCommands()
            {
                var command = new RegisterCommand(
                    "Michael",
                    "Jordan",
                    "MichaelJ@Gmail.com",
                    "Password123",
                    new string('a', PropertiesValidation.User.RoleMaximumLength + 1),
                    new DateOnly(1991, 5, 23));
                Add(command, nameof(command.Role), 1);
            }
        }

        internal class GetInvalidDateOfBirthCommands : TheoryData<RegisterCommand, string, int>
        {
            public GetInvalidDateOfBirthCommands()
            {
                var command = new RegisterCommand(
                    "Michael",
                    "Jordan",
                    "MichaelJ@Gmail.com",
                    "Password123",
                    "Pro Basketball Player",
                    DateOnly.FromDateTime(DateTime.UtcNow));
                Add(command, nameof(command.DateOfBirth), 1);
            }
        }
    }
}
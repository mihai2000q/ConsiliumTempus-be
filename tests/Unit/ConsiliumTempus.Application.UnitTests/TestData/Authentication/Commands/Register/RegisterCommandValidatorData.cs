using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands.Register;

internal static class RegisterCommandValidatorData
{
    internal class GetValidCommands : TheoryData<RegisterCommand>
    {
        public GetValidCommands()
        {
            Add(AuthenticationCommandFactory.CreateRegisterCommand());
            Add(AuthenticationCommandFactory.CreateRegisterCommand(email: "Some@Example.com"));
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
            var command = AuthenticationCommandFactory.CreateRegisterCommand(firstName: "");
            Add(command, nameof(command.FirstName), 1);

            command = AuthenticationCommandFactory.CreateRegisterCommand(
                firstName: new string('a', PropertiesValidation.User.FirstNameMaximumLength + 1));
            Add(command, nameof(command.FirstName), 1);
        }
    }

    internal class GetInvalidLastNameCommands : TheoryData<RegisterCommand, string, int>
    {
        public GetInvalidLastNameCommands()
        {
            var command = AuthenticationCommandFactory.CreateRegisterCommand(lastName: "");
            Add(command, nameof(command.LastName), 1);

            command = AuthenticationCommandFactory.CreateRegisterCommand(
                lastName: new string('a', PropertiesValidation.User.LastNameMaximumLength + 1));
            Add(command, nameof(command.LastName), 1);
        }
    }

    internal class GetInvalidEmailCommands : TheoryData<RegisterCommand, string, int>
    {
        public GetInvalidEmailCommands()
        {
            var command = AuthenticationCommandFactory.CreateRegisterCommand(email: "");
            Add(command, nameof(command.Email), 2);

            command = AuthenticationCommandFactory.CreateRegisterCommand(email: "This is not an email");
            Add(command, nameof(command.Email), 1);
            
            command = AuthenticationCommandFactory.CreateRegisterCommand(email: "SomeExample");
            Add(command, nameof(command.Email), 1);
            
            command = AuthenticationCommandFactory.CreateRegisterCommand(email: "Some@Example");
            Add(command, nameof(command.Email), 1);
            
            command = AuthenticationCommandFactory.CreateRegisterCommand(email: "SomeExample.com");
            Add(command, nameof(command.Email), 1);

            command = AuthenticationCommandFactory.CreateRegisterCommand(
                email: new string('a', PropertiesValidation.User.EmailMaximumLength + 1));
            Add(command, nameof(command.Email), 2);
        }
    }

    internal class GetInvalidPasswordCommands : TheoryData<RegisterCommand, string, int>
    {
        public GetInvalidPasswordCommands()
        {
            var command = AuthenticationCommandFactory.CreateRegisterCommand(password: "");
            Add(command, nameof(command.Password), 5);
            
            command = AuthenticationCommandFactory.CreateRegisterCommand(
                password: new string('a', PropertiesValidation.User.PlainPasswordMaximumLength + 1));
            Add(command, nameof(command.Password), 3);
            
            command = AuthenticationCommandFactory.CreateRegisterCommand(password: "aaa");
            Add(command, nameof(command.Password), 3);
            
            command = AuthenticationCommandFactory.CreateRegisterCommand(password: "Aaaa");
            Add(command, nameof(command.Password), 2);
            
            command = AuthenticationCommandFactory.CreateRegisterCommand(password: "Aaaaaaaaa");
            Add(command, nameof(command.Password), 1);
            
            command = AuthenticationCommandFactory.CreateRegisterCommand(password: "aaaaaaaaa123");
            Add(command, nameof(command.Password), 1);
        }
    }

    internal class GetInvalidRoleCommands : TheoryData<RegisterCommand, string, int>
    {
        public GetInvalidRoleCommands()
        {
            var command = AuthenticationCommandFactory.CreateRegisterCommand(
                role: new string('a', PropertiesValidation.User.RoleMaximumLength + 1));
            Add(command, nameof(command.Role), 1);
        }
    }

    internal class GetInvalidDateOfBirthCommands : TheoryData<RegisterCommand, string, int>
    {
        public GetInvalidDateOfBirthCommands()
        {
            var command = AuthenticationCommandFactory.CreateRegisterCommand(
                dateOfBirth: DateOnly.FromDateTime(DateTime.UtcNow));
            Add(command, nameof(command.DateOfBirth), 1);
        }
    }
}
using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Authentication.Commands.Login;

internal static class LoginCommandValidatorData
{
    internal class GetValidCommands : TheoryData<LoginCommand>
    {
        public GetValidCommands()
        {
            Add(AuthenticationCommandFactory.CreateLoginCommand());
            Add(AuthenticationCommandFactory.CreateLoginCommand(email: "Some@Example.com"));
            Add(new LoginCommand("MichaelJ@Gmail.com", "Password123"));
        }
    }
    
    internal class GetInvalidEmailCommands : TheoryData<LoginCommand, string, int>
    {
        public GetInvalidEmailCommands()
        {
            var command = AuthenticationCommandFactory.CreateLoginCommand(email: "");
            Add(command, nameof(command.Email), 2);
            
            command = AuthenticationCommandFactory.CreateLoginCommand(email: "random string");
            Add(command, nameof(command.Email), 1);
            
            command = AuthenticationCommandFactory.CreateLoginCommand(email: "SomeExample");
            Add(command, nameof(command.Email), 1);
            
            command = AuthenticationCommandFactory.CreateLoginCommand(email: "Some@Example");
            Add(command, nameof(command.Email), 1);
            
            command = AuthenticationCommandFactory.CreateLoginCommand(email: "SomeExample.com");
            Add(command, nameof(command.Email), 1);
            
            command = AuthenticationCommandFactory.CreateLoginCommand(
                email: new string('a', PropertiesValidation.User.EmailMaximumLength + 1));
            Add(command, nameof(command.Email), 2);
        }
    }
    
    internal class GetInvalidPasswordCommands : TheoryData<LoginCommand, string, int>
    {
        public GetInvalidPasswordCommands()
        {
            var command = AuthenticationCommandFactory.CreateLoginCommand(password: "");
            Add(command, nameof(command.Password), 5);
            
            command = AuthenticationCommandFactory.CreateLoginCommand(
                password: new string('a', PropertiesValidation.User.PlainPasswordMaximumLength + 1));
            Add(command, nameof(command.Password), 3);
            
            command = AuthenticationCommandFactory.CreateLoginCommand(password: "aaa");
            Add(command, nameof(command.Password), 3);
            
            command = AuthenticationCommandFactory.CreateLoginCommand(password: "Aaaa");
            Add(command, nameof(command.Password), 2);
            
            command = AuthenticationCommandFactory.CreateLoginCommand(password: "Aaaaaaaaa");
            Add(command, nameof(command.Password), 1);
            
            command = AuthenticationCommandFactory.CreateLoginCommand(password: "aaaaaaaaa123");
            Add(command, nameof(command.Password), 1);
        }
    }
}
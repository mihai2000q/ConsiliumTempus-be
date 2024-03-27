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
            Add(new LoginCommand("MichaelJ@Gmail.com", "Password123"));
        }
    }
    
    internal class GetInvalidEmailCommands : TheoryData<LoginCommand, string, int>
    {
        public GetInvalidEmailCommands()
        {
            var query = AuthenticationCommandFactory.CreateLoginCommand(email: "");
            Add(query, nameof(query.Email), 2);
            
            query = AuthenticationCommandFactory.CreateLoginCommand(email: "random string");
            Add(query, nameof(query.Email), 1);
            
            query = AuthenticationCommandFactory.CreateLoginCommand(
                email: new string('a', PropertiesValidation.User.EmailMaximumLength + 1));
            Add(query, nameof(query.Email), 2);
        }
    }
    
    internal class GetInvalidPasswordCommands : TheoryData<LoginCommand, string, int>
    {
        public GetInvalidPasswordCommands()
        {
            var query = AuthenticationCommandFactory.CreateLoginCommand(password: "");
            Add(query, nameof(query.Password), 5);
            
            query = AuthenticationCommandFactory.CreateLoginCommand(
                password: new string('a', PropertiesValidation.User.PlainPasswordMaximumLength + 1));
            Add(query, nameof(query.Password), 3);
            
            query = AuthenticationCommandFactory.CreateLoginCommand(password: "aaa");
            Add(query, nameof(query.Password), 3);
            
            query = AuthenticationCommandFactory.CreateLoginCommand(password: "Aaaa");
            Add(query, nameof(query.Password), 2);
            
            query = AuthenticationCommandFactory.CreateLoginCommand(password: "Aaaaaaaaa");
            Add(query, nameof(query.Password), 1);
            
            query = AuthenticationCommandFactory.CreateLoginCommand(password: "aaaaaaaaa123");
            Add(query, nameof(query.Password), 1);
        }
    }
}
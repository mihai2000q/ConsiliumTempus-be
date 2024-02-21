using ConsiliumTempus.Application.Authentication.Queries.Login;
using ConsiliumTempus.Common.UnitTests.Authentication;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Authentication.Queries;

internal static class LoginQueryValidatorData
{
    internal class GetValidQueries : TheoryData<LoginQuery>
    {
        public GetValidQueries()
        {
            Add(new LoginQuery("MichaelJ@Gmail.com", "Password123"));
        }
    }
    
    internal class GetInvalidEmailQueries : TheoryData<LoginQuery, string, int>
    {
        public GetInvalidEmailQueries()
        {
            var query = AuthenticationQueryFactory.CreateLoginQuery(email: "");
            Add(query, nameof(query.Email), 2);
            
            query = AuthenticationQueryFactory.CreateLoginQuery(email: "random string");
            Add(query, nameof(query.Email), 1);
            
            query = AuthenticationQueryFactory.CreateLoginQuery(
                email: new string('a', PropertiesValidation.User.EmailMaximumLength + 1));
            Add(query, nameof(query.Email), 2);
        }
    }
    
    internal class GetInvalidPasswordQueries : TheoryData<LoginQuery, string, int>
    {
        public GetInvalidPasswordQueries()
        {
            var query = AuthenticationQueryFactory.CreateLoginQuery(password: "");
            Add(query, nameof(query.Password), 5);
            
            query = AuthenticationQueryFactory.CreateLoginQuery(
                password: new string('a', PropertiesValidation.User.PlainPasswordMaximumLength + 1));
            Add(query, nameof(query.Password), 3);
            
            query = AuthenticationQueryFactory.CreateLoginQuery(password: "aaa");
            Add(query, nameof(query.Password), 3);
            
            query = AuthenticationQueryFactory.CreateLoginQuery(password: "Aaaa");
            Add(query, nameof(query.Password), 2);
            
            query = AuthenticationQueryFactory.CreateLoginQuery(password: "Aaaaaaaaa");
            Add(query, nameof(query.Password), 1);
            
            query = AuthenticationQueryFactory.CreateLoginQuery(password: "aaaaaaaaa123");
            Add(query, nameof(query.Password), 1);
        }
    }
}
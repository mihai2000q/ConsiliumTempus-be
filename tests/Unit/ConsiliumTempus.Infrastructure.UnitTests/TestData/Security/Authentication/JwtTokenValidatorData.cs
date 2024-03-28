using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authentication;

internal static class JwtTokenValidatorData
{
    internal class GetInvalidTokensByJwtSettings : TheoryData<string>
    {
        private readonly JwtSettings _jwtSettings = new()
        {
            SecretKey = "This-Is-My-Secret-Duper-Key-And-I-Like-It, This-Is-My-Secret-Duper-Key-And-I-Like-It",
            Audience = "Audience",
            Issuer = "Issuer",
            ExpiryMinutes = 7
        };

        public GetInvalidTokensByJwtSettings()
        {
            var token = Utils.Token.GenerateToken(_jwtSettings, algorithm: SecurityAlgorithms.HmacSha384);
            Add(token);
            
            token = Utils.Token.GenerateToken(_jwtSettings, issuer: "a");
            Add(token);
            
            token = Utils.Token.GenerateToken(_jwtSettings, audience: "a");
            Add(token);
        }
    }
    
    internal class GetInvalidTokensByClaims : TheoryData<string, UserAggregate>
    {
        private readonly JwtSettings _jwtSettings = new()
        {
            SecretKey = "This-Is-My-Secret-Duper-Key-And-I-Like-It",
            Audience = "Audience",
            Issuer = "Issuer",
            ExpiryMinutes = 7
        };

        public GetInvalidTokensByClaims()
        {
            var user = UserFactory.Create();
            var token = Utils.Token.GenerateToken(_jwtSettings, user, email: "");
            Add(token, user);
            
            token = Utils.Token.GenerateToken(_jwtSettings, user, givenName: "");
            Add(token, user);
            
            token = Utils.Token.GenerateToken(_jwtSettings, user, familyName: "");
            Add(token, user);
            
            token = Utils.Token.GenerateToken(_jwtSettings, user, jti: "");
            Add(token, user);
        }
    }
}
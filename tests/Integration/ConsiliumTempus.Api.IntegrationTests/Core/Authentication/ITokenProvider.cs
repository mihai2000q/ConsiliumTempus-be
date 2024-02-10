using System.IdentityModel.Tokens.Jwt;

namespace ConsiliumTempus.Api.IntegrationTests.Core.Authentication;

public interface ITokenProvider
{
    JwtSecurityToken? GetToken();

    void SetToken(JwtSecurityToken? token);
}
using System.IdentityModel.Tokens.Jwt;

namespace ConsiliumTempus.Api.IntegrationTests.Core.Authentication;

public class TokenProvider
{
    private JwtSecurityToken? _token;
    
    public JwtSecurityToken? GetToken()
    {
        return _token;
    }

    public void SetToken(JwtSecurityToken? token)
    {
        _token = token;
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ConsiliumTempus.Infrastructure.UnitTests.Mock;

internal static partial class Mock
{
    internal static class Token
    {
        public static string CreateMock(params (string, string)[] claims)
        {
            var tokenClaims = claims.Select(c => new Claim(c.Item1, c.Item2)).ToArray();

            var securityToken = new JwtSecurityToken(claims: tokenClaims);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
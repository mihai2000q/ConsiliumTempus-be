using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class Token
    {
        internal static JwtSecurityToken CreateMock(UserAggregate user, JwtSettings jwtSettings)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Credentials.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName.Value),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityToken = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                expires: DateTime.UtcNow.AddHours(jwtSettings.ExpiryHours),
                claims: claims,
                signingCredentials: signingCredentials);

            return securityToken;
        }

        internal static JwtSecurityToken CreateInvalidToken(JwtSettings jwtSettings)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, ""),
                new Claim(JwtRegisteredClaimNames.Email, ""),
                new Claim(JwtRegisteredClaimNames.GivenName, ""),
                new Claim(JwtRegisteredClaimNames.FamilyName, ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityToken = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                expires: DateTime.UtcNow.AddHours(jwtSettings.ExpiryHours),
                claims: claims,
                signingCredentials: signingCredentials);

            return securityToken;
        }

        internal static string SecurityTokenToStringToken(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
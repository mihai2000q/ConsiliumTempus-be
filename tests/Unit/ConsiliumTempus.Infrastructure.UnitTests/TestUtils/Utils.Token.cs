using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Token
    {
        internal static string GenerateToken(
            JwtSettings jwtSettings,
            UserAggregate? user = null,
            string? algorithm = null,
            string? issuer = null,
            string? audience = null,
            string? sub = null,
            string? email = null,
            string? jti = null)
        {
            user ??= UserFactory.Create();

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                algorithm ?? SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, sub ?? user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email ?? user.Credentials.Email),
                new Claim(JwtRegisteredClaimNames.Jti, jti ?? Guid.NewGuid().ToString())
            };

            var securityToken = new JwtSecurityToken(
                issuer: issuer ?? jwtSettings.Issuer,
                audience: audience ?? jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
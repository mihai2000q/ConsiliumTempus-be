using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

public static partial class Utils
{
    public static class Token
    {
        public static string CreateMock(UserAggregate user, JwtSettings jwtSettings)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Credentials.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name.First),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Name.Last),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityToken = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                expires: DateTime.UtcNow.AddHours(jwtSettings.ExpiryHours),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public static string CreateInvalidToken(JwtSettings jwtSettings)
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

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}